﻿using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

using TwitchBot.Extensions;
using TwitchBot.Models;

namespace TwitchBot.Repositories
{
    public class BankRepository
    {
        private string _connStr;

        public BankRepository(string connStr)
        {
            _connStr = connStr;
        }

        public void CreateAccount(string recipient, int broadcasterId, int deposit)
        {
            string query = "INSERT INTO Bank (Username, Wallet, Broadcaster) VALUES (@username, @wallet, @broadcaster)";

            using (SqlConnection conn = new SqlConnection(_connStr))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.Add("@username", SqlDbType.VarChar, 30).Value = recipient;
                cmd.Parameters.Add("@wallet", SqlDbType.Int).Value = deposit;
                cmd.Parameters.Add("@broadcaster", SqlDbType.Int).Value = broadcasterId;

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateFunds(string walletOwner, int broadcasterId, int newWalletBalance)
        {
            string query = "UPDATE Bank SET Wallet = @wallet WHERE (Username = @username AND Broadcaster = @broadcaster)";

            using (SqlConnection conn = new SqlConnection(_connStr))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.Add("@wallet", SqlDbType.Int).Value = newWalletBalance;
                cmd.Parameters.Add("@username", SqlDbType.VarChar, 30).Value = walletOwner;
                cmd.Parameters.Add("@broadcaster", SqlDbType.Int).Value = broadcasterId;

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public DataTable UpdateCreateBalance(List<string> usernameList, int broadcasterId, int deposit, bool showOutput = false)
        {
            DataTable usernamesTable = usernameList.ToDataTable();
            DataTable resultTable = new DataTable();

            using (SqlConnection conn = new SqlConnection(_connStr))
            using (SqlCommand cmd = new SqlCommand("UpdateCreateBalance", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@tvpUsernames", SqlDbType.Structured)).Value = usernamesTable;
                cmd.Parameters.Add("@intBroadcasterID", SqlDbType.Int).Value = broadcasterId;
                cmd.Parameters.Add("@intDeposit", SqlDbType.Int).Value = deposit;
                cmd.Parameters.Add("@bitShowOutput", SqlDbType.Bit).Value = showOutput;

                using (SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd))
                {
                    dataAdapter.Fill(resultTable);
                }
            }

            return resultTable;
        }

        public int CheckBalance(string username, int broadcasterId)
        {
            using (SqlConnection conn = new SqlConnection(_connStr))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Bank WHERE Broadcaster = @broadcaster", conn))
                {
                    cmd.Parameters.Add("@broadcaster", SqlDbType.Int).Value = broadcasterId;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                if (username.Equals(reader["Username"].ToString()))
                                {
                                    return int.Parse(reader["Wallet"].ToString());
                                }
                            }
                        }
                    }
                }
            }

            return -1;
        }

        public List<BalanceResult> GetCurrencyLeaderboard(string broadcasterName, int broadcasterId, string botName)
        {
            List<BalanceResult> balanceResult = new List<BalanceResult>();

            using (SqlConnection conn = new SqlConnection(_connStr))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT TOP 3 * FROM Bank " 
                    + "WHERE Broadcaster = @broadcasterId AND Username <> @broadcasterName AND Username <> @botName "
                    + "ORDER BY Wallet DESC", conn))
                {
                    cmd.Parameters.Add("@broadcasterId", SqlDbType.Int).Value = broadcasterId;
                    cmd.Parameters.Add("@broadcasterName", SqlDbType.VarChar, 30).Value = broadcasterName;
                    cmd.Parameters.Add("@botName", SqlDbType.VarChar, 30).Value = botName;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                balanceResult.Add(new BalanceResult
                                {
                                    ActionType = "SELECT",
                                    Username = reader["Username"].ToString(),
                                    Wallet = int.Parse(reader["Wallet"].ToString())
                                });
                            }
                        }
                    }
                }
            }

            return balanceResult;
        }
    }
}
