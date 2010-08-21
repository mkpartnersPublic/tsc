using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;

namespace WebServiceTutorial
{
    public class BusinessRules
    {
        public Verses Retrieve(int id)
        {
            // create instance of Verseus struct with default constructor
            Verses newVerses = new Verses();
            //string strConnection = HelperClass.RetrieveConnectionString();
            string connectionString;            
            connectionString = "Server = .\\EDWIN; Database = Inventory; User ID = sa; Password = Edwin; Trusted_Connection = False";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Create the command and set its properties.
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "ReturnVerse";
                command.CommandType = CommandType.StoredProcedure;

                // Add the input parameter and set its properties.
                SqlParameter parameter = new SqlParameter();
                parameter.ParameterName = "@id";
                parameter.SqlDbType = SqlDbType.Int;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = id;

                // Add the parameter to the Parameters collection. 
                command.Parameters.Add(parameter);



                // add logic to connect to the database and retrieve the Verses
                // return new point struct


                SqlDataAdapter adapter = new SqlDataAdapter();

                // A table mapping names the DataTable.
                adapter.TableMappings.Add("Table", "TblVerse");

                // Open the connection.
                connection.Open();
                
                // Set the SqlDataAdapter's SelectCommand.
                adapter.SelectCommand = command;

                // Fill the DataSet.
                DataSet dataSet = new DataSet("Verses");
                adapter.Fill(dataSet);

                // Close the connection.
                connection.Close();

                newVerses.book = dataSet.Tables[0].Rows[0]["book"].ToString();
                newVerses.id = id;
                newVerses.text = dataSet.Tables[0].Rows[0]["text"].ToString();
                newVerses.chapter = Convert.ToInt32(dataSet.Tables[0].Rows[0]["chapter"].ToString());
                newVerses.verse = Convert.ToInt32(dataSet.Tables[0].Rows[0]["verse"].ToString());
                newVerses.error = "";
                newVerses.returnValue = "True";
                return newVerses;
            }
        }

        public InventoryResponse getVersePrice(int projectId, int languageId, Boolean isGroup)
        {
            // create instance of Verseus struct with default constructor
            InventoryResponse newInventoryResponse = new InventoryResponse();
            newInventoryResponse.initialize();
            string connectionString = HelperClass.RetrieveConnectionString();
            //string connectionString;
            //connectionString = "Server = .\\EDWIN; Database = Inventory; User ID = sa; Password = Edwin; Trusted_Connection = False";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Create the command and set its properties.
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "GetVersePrice";
                command.CommandType = CommandType.StoredProcedure;

                // Add the input parameter and set its properties.
                SqlParameter parameter1 = new SqlParameter();
                parameter1.ParameterName = "@ProjectId";
                parameter1.SqlDbType = SqlDbType.Int;
                parameter1.Direction = ParameterDirection.Input;
                parameter1.Value = projectId;

                // Add the parameter to the Parameters collection. 
                command.Parameters.Add(parameter1);


                // Add the input parameter and set its properties.
                SqlParameter parameter2 = new SqlParameter();
                parameter2.ParameterName = "@languageId";
                parameter2.SqlDbType = SqlDbType.Int;
                parameter2.Direction = ParameterDirection.Input;
                parameter2.Value = languageId;

                // Add the parameter to the Parameters collection. 
                command.Parameters.Add(parameter2);


                // Add the input parameter and set its properties.
                SqlParameter parameter3 = new SqlParameter();
                parameter3.ParameterName = "@IsGroup";
                parameter3.SqlDbType = SqlDbType.Bit;
                parameter3.Direction = ParameterDirection.Input;
                parameter3.Value = isGroup;

                // Add the parameter to the Parameters collection. 
                command.Parameters.Add(parameter3);


                // add logic to connect to the database and retrieve the prices
                // return new point struct


                SqlDataAdapter adapter = new SqlDataAdapter();

                // A table mapping names the DataTable.
                adapter.TableMappings.Add("Table", "TblVerse");

                // Open the connection.
                connection.Open();

                // Set the SqlDataAdapter's SelectCommand.
                adapter.SelectCommand = command;

                // Fill the DataSet.
                DataSet dataSet = new DataSet("Verses");
                adapter.Fill(dataSet);

                // Close the connection.
                connection.Close();

                newInventoryResponse.sucess = true;
                newInventoryResponse.error = "";


                if (dataSet.Tables[0].Rows.Count > 0)
                {
                    VersePrice vp = new VersePrice();
                    vp.versePrice = Convert.ToDecimal(dataSet.Tables[0].Rows[0]["VERSE_PRICE"].ToString());
                    vp.sucess = true;
                    newInventoryResponse.addVersePrice(vp);
                }
                else
                {
                    newInventoryResponse.error = "No verse price available for this project and language";
                }

                return newInventoryResponse;
                //newInventoryResponse.VersePrice = Convert.ToDecimal(dataSet.Tables[0].Rows[0]["VERSE_PRICE"].ToString());
            }
        }

        public InventoryResponse getVersesAssignedByDonationId(String sfdcDonationId)
        {
            // create instance of Verseus struct with default constructor
            InventoryResponse newInventoryResponse = new InventoryResponse();
            newInventoryResponse.initialize();
            string connectionString = HelperClass.RetrieveConnectionString();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Create the command and set its properties.
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "getVersesAssignedByDonationId";
                command.CommandType = CommandType.StoredProcedure;

                // Add the input parameter and set its properties.
                SqlParameter parameter1 = new SqlParameter();
                parameter1.ParameterName = "@sfdcDonationId";
                parameter1.SqlDbType = SqlDbType.VarChar;
                parameter1.Direction = ParameterDirection.Input;
                parameter1.Value = sfdcDonationId;

                // Add the parameter to the Parameters collection. 
                command.Parameters.Add(parameter1);




                // add logic to connect to the database and retrieve the verses
                

                SqlDataAdapter adapter = new SqlDataAdapter();

                // A table mapping names the DataTable.
                adapter.TableMappings.Add("Table", "TblVerse");

                // Open the connection.
                connection.Open();

                // Set the SqlDataAdapter's SelectCommand.
                adapter.SelectCommand = command;

                // Fill the DataSet.
                DataSet dataSet = new DataSet("Verses");
                adapter.Fill(dataSet);

                // Close the connection.
                connection.Close();

                newInventoryResponse.sucess = true;
                newInventoryResponse.error = "";


                if (dataSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dataSet.Tables[0].Rows)
                    {
                        Verses addNewVerse = new Verses();
                        addNewVerse.book = dr["book"].ToString();
                        addNewVerse.chapter = Convert.ToInt16(dr["chapter"].ToString());
                        addNewVerse.id = Convert.ToInt16(dr["id"].ToString());
                        addNewVerse.verse = Convert.ToInt16(dr["verse"].ToString());
                        addNewVerse.returnValue = "True";
                        addNewVerse.text = dr["text"].ToString();
                        //addNewVerse.verseGroupId = "";
                        newInventoryResponse.addVerse(addNewVerse);
                    }

                }
                else
                {
                    newInventoryResponse.error = "No verses available for this DonationId";
                }

                return newInventoryResponse;
            }
        }

        public InventoryResponse getVersesAssignedByAuthId(int authId)
        {
            // create instance of Verseus struct with default constructor
            InventoryResponse newInventoryResponse = new InventoryResponse();
            newInventoryResponse.initialize();
            string connectionString = HelperClass.RetrieveConnectionString();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Create the command and set its properties.
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "getVersesAssignedByAuthId";
                command.CommandType = CommandType.StoredProcedure;

                // Add the input parameter and set its properties.
                SqlParameter parameter1 = new SqlParameter();
                parameter1.ParameterName = "@authId";
                parameter1.SqlDbType = SqlDbType.BigInt;
                parameter1.Direction = ParameterDirection.Input;
                parameter1.Value = authId;

                // Add the parameter to the Parameters collection. 
                command.Parameters.Add(parameter1);




                // add logic to connect to the database and retrieve the verses


                SqlDataAdapter adapter = new SqlDataAdapter();

                // A table mapping names the DataTable.
                adapter.TableMappings.Add("Table", "TblVerse");

                // Open the connection.
                connection.Open();

                // Set the SqlDataAdapter's SelectCommand.
                adapter.SelectCommand = command;

                // Fill the DataSet.
                DataSet dataSet = new DataSet("Verses");
                adapter.Fill(dataSet);

                // Close the connection.
                connection.Close();

                newInventoryResponse.sucess = true;
                newInventoryResponse.error = "";


                if (dataSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dataSet.Tables[0].Rows)
                    {
                        Verses addNewVerse = new Verses();
                        addNewVerse.book = dr["book"].ToString();
                        addNewVerse.chapter = Convert.ToInt16(dr["chapter"].ToString());
                        addNewVerse.id = Convert.ToInt16(dr["id"].ToString());
                        addNewVerse.verse = Convert.ToInt16(dr["verse"].ToString());
                        addNewVerse.returnValue = "True";
                        addNewVerse.text = dr["text"].ToString();
                        //addNewVerse.verseGroupId = "";
                        newInventoryResponse.addVerse(addNewVerse);
                    }

                }
                else
                {
                    newInventoryResponse.error = "No verses available for this AuthId";
                }

                return newInventoryResponse;
            }
        }

        public InventoryResponse releaseVersesAssignedByDonationId(String sfdcDonationId)
        {
            // create instance of Verseus struct with default constructor
            InventoryResponse newInventoryResponse = new InventoryResponse();
            newInventoryResponse.initialize();
            string connectionString = HelperClass.RetrieveConnectionString();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Create the command and set its properties.
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "releaseVersesAssignedByDonationId";
                command.CommandType = CommandType.StoredProcedure;

                // Add the input parameter and set its properties.
                SqlParameter parameter1 = new SqlParameter();
                parameter1.ParameterName = "@sfdcDonationId";
                parameter1.SqlDbType = SqlDbType.VarChar;
                parameter1.Direction = ParameterDirection.Input;
                parameter1.Value = sfdcDonationId;

                // Add the parameter to the Parameters collection. 
                command.Parameters.Add(parameter1);

                // Open the connection.
                connection.Open();

                int rows = command.ExecuteNonQuery();

                connection.Close();

                newInventoryResponse.sucess = true;
                newInventoryResponse.error = "";


                if (rows > 0)
                {

                    newInventoryResponse.error = "";
                    newInventoryResponse.sucess = true;
                }
                else
                {
                    newInventoryResponse.error = "No verses released for this sfdcDonationId";
                }

                return newInventoryResponse;
            }
        }

        public InventoryResponse releaseVersesAssignedByAuthId(int authId)
        {
            // create instance of Verseus struct with default constructor
            InventoryResponse newInventoryResponse = new InventoryResponse();
            newInventoryResponse.initialize();
            string connectionString = HelperClass.RetrieveConnectionString();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Create the command and set its properties.
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "releaseVersesAssignedByAuthId";
                command.CommandType = CommandType.StoredProcedure;

                // Add the input parameter and set its properties.
                SqlParameter parameter1 = new SqlParameter();
                parameter1.ParameterName = "@authId";
                parameter1.SqlDbType = SqlDbType.BigInt;
                parameter1.Direction = ParameterDirection.Input;
                parameter1.Value = authId;

                // Add the parameter to the Parameters collection. 
                command.Parameters.Add(parameter1);

                // Open the connection.
                connection.Open();

                int rows = command.ExecuteNonQuery();

                connection.Close();

                newInventoryResponse.sucess = true;
                newInventoryResponse.error = "";


                if (rows > 0)
                {

                    newInventoryResponse.error = "";
                    newInventoryResponse.sucess = true;
                }
                else
                {
                    newInventoryResponse.error = "No verses released for this AuthId";
                }

                return newInventoryResponse;
            }
        }

        public InventoryResponse assignVerses(int projectId, int languageId, String sfdcDonationId, String sfdcContactId, String fundId, DateTime paymentDate, int authId, int numberOfVerses)
        {
            // create instance of Verseus struct with default constructor
            InventoryResponse newInventoryResponse = new InventoryResponse();
            newInventoryResponse.initialize();
            string connectionString = HelperClass.RetrieveConnectionString();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Create the command and set its properties.
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "AssignVerses";
                command.CommandType = CommandType.StoredProcedure;

                // Add the input parameters and set its properties.
                SqlParameter parameter1 = new SqlParameter();
                parameter1.ParameterName = "@ProjectId";
                parameter1.SqlDbType = SqlDbType.Int;
                parameter1.Direction = ParameterDirection.Input;
                parameter1.Value = projectId;

                SqlParameter parameter2 = new SqlParameter();
                parameter2.ParameterName = "@languageId";
                parameter2.SqlDbType = SqlDbType.Int;
                parameter2.Direction = ParameterDirection.Input;
                parameter2.Value = languageId;

                SqlParameter parameter3 = new SqlParameter();
                parameter3.ParameterName = "@sfdcContactId";
                parameter3.SqlDbType = SqlDbType.VarChar;
                parameter3.Direction = ParameterDirection.Input;
                parameter3.Value = sfdcContactId;
                                
                SqlParameter parameter4 = new SqlParameter();
                parameter4.ParameterName = "@sfdcDonationId";
                parameter4.SqlDbType = SqlDbType.VarChar;
                parameter4.Direction = ParameterDirection.Input;
                parameter4.Value = sfdcDonationId;
                
                SqlParameter parameter5 = new SqlParameter();
                parameter5.ParameterName = "@fundId";
                parameter5.SqlDbType = SqlDbType.VarChar;
                parameter5.Direction = ParameterDirection.Input;
                parameter5.Value = fundId;

                SqlParameter parameter6 = new SqlParameter();
                parameter6.ParameterName = "@paymentDate";
                parameter6.SqlDbType = SqlDbType.DateTime;
                parameter6.Direction = ParameterDirection.Input;
                parameter6.Value = paymentDate;

                SqlParameter parameter7 = new SqlParameter();
                parameter7.ParameterName = "@authId";
                parameter7.SqlDbType = SqlDbType.BigInt;
                parameter7.Direction = ParameterDirection.Input;
                parameter7.Value = authId;

                SqlParameter parameter8 = new SqlParameter();
                parameter8.ParameterName = "@numberOfVerses";
                parameter8.SqlDbType = SqlDbType.Int;
                parameter8.Direction = ParameterDirection.Input;
                parameter8.Value = numberOfVerses;

                // Add the parameter to the Parameters collection. 
                command.Parameters.Add(parameter1);
                command.Parameters.Add(parameter2);
                command.Parameters.Add(parameter3);
                command.Parameters.Add(parameter4);
                command.Parameters.Add(parameter5);
                command.Parameters.Add(parameter6);
                command.Parameters.Add(parameter7);
                command.Parameters.Add(parameter8);

                SqlDataAdapter adapter = new SqlDataAdapter();

                // A table mapping names the DataTable.
                adapter.TableMappings.Add("Table", "TblVerse");

                // Open the connection.
                connection.Open();

                // Set the SqlDataAdapter's SelectCommand.
                adapter.SelectCommand = command;

                // Fill the DataSet.
                DataSet dataSet = new DataSet("Verses");
                adapter.Fill(dataSet);

                // Close the connection.
                connection.Close();

                newInventoryResponse.sucess = true;
                newInventoryResponse.error = "";

                if (dataSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dataSet.Tables[0].Rows)
                    {
                        Verses addNewVerse = new Verses();
                        addNewVerse.book = dr["book"].ToString();
                        addNewVerse.chapter = Convert.ToInt16(dr["chapter"].ToString());
                        addNewVerse.id = Convert.ToInt16(dr["id"].ToString());
                        addNewVerse.verse = Convert.ToInt16(dr["verse"].ToString());
                        addNewVerse.returnValue = "True";
                        addNewVerse.text = dr["text"].ToString();
                        //addNewVerse.verseGroupId = "";
                        newInventoryResponse.addVerse(addNewVerse);
                    }

                }
                else
                {
                    newInventoryResponse.error = "No verses Assigned";
                }

                return newInventoryResponse;
            }
        }
    }
}
