using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Transactions;
using System.Web.Http;
using eMailService.Helps;
using eMailService.IRepository;
using eMailService.Models;
using eMailService.Results;

namespace eMailService.Repository
{
    public class EmailRepository : IEmailRepository
    {
        public void Create(EmailRecord emailRecord)
        {
            try
            {
                emailRecord.Id = Guid.NewGuid();
                var command = new SqlCommand
                {
                    CommandText = string.Format("insert into Email (id, data) values (@id, @data)"),
                };
                command.Parameters.Add("@id", SqlDbType.UniqueIdentifier);
                command.Parameters.Add("@data", SqlDbType.NVarChar);
                command.Parameters["@id"].Value = emailRecord.Id;
                command.Parameters["@data"].Value = emailRecord.Data;

                using (var connection = Helpers.NewConnection())
                {
                    connection.Open();
                    command.Connection = connection;
                    using (var transactionScope = new TransactionScope())
                    {
                        command.ExecuteNonQuery();
                        transactionScope.Complete();
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void Delete(Guid Id)
        {
            try
            {
                var command = new SqlCommand
                {
                    CommandText = string.Format("delete from Email where id = @id"),
                };
                command.Parameters.Add(new SqlParameter("@id", Id));
                using (var connection = Helpers.NewConnection())
                {
                    connection.Open();
                    command.Connection = connection;
                    using (var transactionScope = new TransactionScope())
                    {
                        command.ExecuteNonQuery();
                        transactionScope.Complete();
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public List<EmailRecord> Get()
        {
            try
            {
                var records = new List<EmailRecord>();
                var command = new SqlCommand
                {
                    CommandText = string.Format("select * from Email")
                };
                using (var connection = Helpers.NewConnection())
                {
                    connection.Open();
                    command.Connection = connection;
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            records.Add(new EmailRecord
                            {
                                Id = Guid.Parse(reader["Id"].ToString()),
                                Data = reader["Data"].ToString()
                            });
                        }
                    }
                }
                return records;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public EmailRecord Get(Guid Id)
        {
            try
            {
                EmailRecord record = null;
                var command = new SqlCommand
                {
                    CommandText = string.Format("select Top(1) * from Email where id = @id")
                };
                command.Parameters.Add(new SqlParameter("@id", Id));

                using (var connection = Helpers.NewConnection())
                {
                    connection.Open();
                    command.Connection = connection;
                    using (var reader = command.ExecuteReader())
                    {
                        if (!reader.Read())
                        {
                            throw new Exception("throw exception while get data");
                        }
                        record = new EmailRecord
                        {
                            Id = Guid.Parse(reader["Id"].ToString()),
                            Data = reader["Data"].ToString()
                        };
                    }
                }
                return record;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}