using System;
using MySql.Data.MySqlClient;

namespace Assignment6
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            // Print a series of options between 1 and 4 for the user to choose in order to manipulate the database
            Console.WriteLine("Welcome to the Client's Contract Database Consultor!");
            Console.WriteLine("===================================================== \n ");
            Console.WriteLine("To see a view of the overall database, please press 1\n");
            Console.WriteLine("To view an example of data being inserted into the database, please press 2\n");
            Console.WriteLine("To view an update of the database, please press 3\n");
            Console.WriteLine("To view an example of data being deleted from the database, please press 4\n");
            Console.WriteLine("To calculate the overall average number of contracts per client, please press 5\n");
            Console.WriteLine("To calculate the average duration of a contract, please press 6\n");
            Console.WriteLine("To estimate the time remaining on a specific contract, please press 7\n");
            Console.WriteLine("To calculate the average contract value per client, please press 8\n");
            Console.WriteLine("To calculate the total number of contracts open currently, please press 9\n");


            // Connecting to the database
            int caseSwitch = int.Parse(Console.ReadLine());
            string connString = "Server=127.0.0.1" + ";Database=Assignment6"
                       + ";port=3306" + ";User Id=root" + ";password=Kinder1987";
            MySqlConnection conn = new MySqlConnection(connString);
            MySqlDataReader rdr = null;
            MySqlCommand cmd;
            try
            {
                // Opening a connection
                conn.Open();

                // Switch statement in order to carry out different manipulations of 
                // the database depending on the option number the user chooses
                switch (caseSwitch)
                {

                    // Case 1 shows an overall view of the database
                    case 1:
                        Console.WriteLine("Client's Contract Database ");
                        Console.WriteLine("============================ \n ");
                        // 2. READ FROM THE CONNECTION
                        // Use the connection to get query results
                        cmd = new MySqlCommand("select * from Contract_Clients", conn);
                        rdr = cmd.ExecuteReader();

                        while (rdr.Read())
                        {
                            // get the results of each column
                            int Client_ID = (int)rdr["Client_ID"];
                            string Client_Name = (string)rdr["Client_Name"];
                            string DOB = (string)rdr["DOB"];
                            string Address = (string)rdr["Address"];

                            // print out the results
                            Console.Write("{0,-25}", Client_ID);
                            Console.Write("{0,-20}", Client_Name);
                            Console.Write("{0,-25}", DOB);
                            Console.Write("{0, -20}", Address);
                            Console.WriteLine();
                        }
                        break;

                    // Case 2 shows the user an example of some data being inserted into the database
                    case 2:
                        Console.WriteLine("To show data being inserted into the Database ");
                        Console.WriteLine("============================================== \n ");
                        Console.WriteLine("Please enter a Client name and surname: ");
                        string clientName = Console.ReadLine();
                        Console.WriteLine("Please enter a Client date of birth in the format DD/MM/YYYY: ");
                        string clientDOB = Console.ReadLine();
                        Console.WriteLine("Please enter a Client address (example format: Rahoon, Galway): ");
                        string clientAddress = Console.ReadLine();
                        // INSERTING DATA
                        // prepare command string
                        string insertString = @"
                         insert into Contract_Clients
                         (Client_Name, DOB, Address)
                         values ('" + clientName + "', '" + clientDOB + "', '" + clientAddress + "')";

                        // Instantiate a new command with the query and connection
                        cmd = new MySqlCommand(insertString, conn);

                        // Call ExecuteNonQuery to send command
                        int result = cmd.ExecuteNonQuery();
                        if (result == 0)
                        {
                            Console.WriteLine("There was a problem inserting the data");
                        }
                        else
                        {
                            Console.WriteLine("\n Data inserted successfully!");
                        }
                        break;

                    // Case 3 shows the user an example of an update to the database
                    // In this case the date of birth will be updated
                    case 3:
                        Console.WriteLine("To show data being updated in the database ");
                        Console.WriteLine("========================================== \n ");
                        Console.WriteLine("Please enter a new Client DOB in the format DD/MM/YYYY: ");
                        string updatedClientDOB = Console.ReadLine();
                        Console.WriteLine("Please enter a Client Id number between 1 and 4: ");
                        int clientId = int.Parse(Console.ReadLine());
                        // UPDATING DATA
                        // prepare command string
                        string updateString = @"
                        update Contract_Clients
                        set DOB = '" + updatedClientDOB + "' where Client_ID = '" + clientId + "' ";

                        // Instantiate a new command with command text only
                        cmd = new MySqlCommand(updateString);

                        // Set the Connection property
                        cmd.Connection = conn;

                        // Call ExecuteNonQuery to send command
                        int updateResult = cmd.ExecuteNonQuery();
                        if (updateResult == 0)
                        {
                            Console.WriteLine("\nThere was a problem updating the data");
                        }
                        else
                        {
                            Console.WriteLine("\nData updated successfully!");
                        }
                        break;

                    // Case 4 shows an example of some data being deleted from the database
                    // In this case a Client name will be deleted from the database
                    case 4:
                        Console.WriteLine("To show data being deleted from the database ");
                        Console.WriteLine("============================================= \n ");
                        Console.WriteLine("Please enter a Client Name in the format (Name Surname) to delete from the database: ");
                        string deleteClientName = Console.ReadLine();

                        // 4. DELETING DATA
                        // prepare command string
                        string deleteString = @"
                        delete from Contract_Clients where Client_Name = '" + deleteClientName + "'";

                        // Instantiate a new command
                        cmd = new MySqlCommand();

                        // Set the CommandText Property
                        cmd.CommandText = deleteString;

                        // Set the Connection Property
                        cmd.Connection = conn;

                        // Call ExecuteNonQuery to send command
                        int deleteResult = cmd.ExecuteNonQuery();
                        if (deleteResult == 0)
                        {
                            Console.WriteLine("\nThere was a problem deleting the data");
                        }
                        else
                        {
                            Console.WriteLine("\nData deleted successfully!");
                        }
                        break;


                    // Case 5 calculates the overall average number of contracts per client
                    case 5:
                        GetAverageContractsPerClient(rdr, conn);
                        break;


                    // Case 6 calculates the average duration of a contract
                    case 6:
                        CalcAvgContractDuration(rdr, conn);
                        break;
                       

                    // Case 7 estimates the time remaining on a specific contract
                    case 7:
                        EstimatedRemainingTimeOnContract(rdr, conn);
                        break;

                    // Case 8 calculates the average contract value per client
                    case 8:
                        CalcAvgContractValPerClient(rdr, conn);
                        break;


                    // Case 9 calculates the total number of contracts open currently
                    case 9:
                        CalcTotalNumCurrContractsOpen(rdr, conn);
                        break;
                }




            }


            finally

            {
                // To disconnect from the database
                Console.WriteLine("\nThe Client's Contract Database is now disconnected!");
                Console.WriteLine("===================================================== \n ");
                // close the reader
                if (rdr != null)
                {
                    rdr.Close();
                }


                // CLOSE THE CONNECTION
                if (conn != null)
                {
                    conn.Close();
                }
            }
                    }


        private static void GetAverageContractsPerClient(MySqlDataReader rdr, MySqlConnection conn) 
        {

            Console.WriteLine("The overall average number of contracts per client: ");
            Console.WriteLine("==================================================== \n ");
            MySqlCommand cmd;
            // 5. Calculate the overall average number of contracts per client
            // prepare command string

            cmd = new MySqlCommand("select TRUNCATE(avg(n),6)  AS average from(select count(s.Client_Id) as n " +
                "from Contract_Clients u left outer join Contracts s on u.Client_Id = s.Client_Id group by u.Client_Id) as dt", conn);
            rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                Console.WriteLine("\nThe average number of contracts per client is: " + rdr["average"]);

            }
        }


        private static void CalcAvgContractDuration(MySqlDataReader rdr, MySqlConnection conn)
        {

            Console.WriteLine("The average duration of a contract: ");
            Console.WriteLine("===================================== \n ");
            MySqlCommand cmd;

            // 6. To calculate the average duration of a contract
            // prepare command string
            cmd = new MySqlCommand("select truncate(avg(ACD),0) as cda from(select DATEDIFF(Contract_End_Date, Contract_Start_Date) as ACD from Contracts) as dt", conn);
            rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                Console.WriteLine("\nThe average duration of a contract is: " + rdr["cda"] + " days");
            }

        }


        private static void EstimatedRemainingTimeOnContract(MySqlDataReader rdr, MySqlConnection conn)
        {

            Console.WriteLine("Estimated Time remaining on a specific contract: ");
            Console.WriteLine("================================================== \n ");
            MySqlCommand cmd;

            // 7. To estimate the time remaining on a specific contract
            // prepare command string
            cmd = new MySqlCommand("select DATEDIFF(Contract_End_Date, NOW())  as ACD, idContracts from Contracts order by idContracts asc;", conn);
            rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                int remainingDays = rdr.GetInt32("ACD");//(int)rdr["ACD"];
                if (remainingDays < 0)
                {
                    Console.WriteLine("The contract with id: " + rdr["idContracts"] + " expired " + remainingDays + " days ago");
                }
                else
                {
                    Console.WriteLine("The contract  with id: " + rdr["idContracts"] + " will expire in " + remainingDays + " days");
                }

            }

        }


        private static void CalcAvgContractValPerClient(MySqlDataReader rdr, MySqlConnection conn)
        {
            Console.WriteLine("Average contract value per client: ");
            Console.WriteLine("==================================== \n ");
            MySqlCommand cmd;

            // prepare command string
            cmd = new MySqlCommand("select truncate(avg(CV),2) as ACD from (select Contract_Value as CV from Contracts order by idContracts asc) as dt", conn);
            rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                Console.WriteLine("\nThe Average Contract Value per Client is: €" + rdr["ACD"]);
            }

        }


        private static void CalcTotalNumCurrContractsOpen(MySqlDataReader rdr, MySqlConnection conn)
        {
            Console.WriteLine("Total number of contracts open currently: ");
            Console.WriteLine("========================================== \n ");
            MySqlCommand cmd;

            // 9. To calculate the total number of contracts open currently
            // prepare command string
            cmd = new MySqlCommand(" select count(*) AS 'TNC'  from Contracts where DATEDIFF(Contract_End_Date, NOW())  > 0 order by idContracts asc;", conn);
            rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                Console.WriteLine("\nThe total number of contracts open currently is: " + rdr["TNC"]);
            }

        }

    }

}

          


    
