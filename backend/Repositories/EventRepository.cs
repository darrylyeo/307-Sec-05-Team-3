﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using backend.Exceptions;
using System.Configuration;
using backend.DTOs;
using backend.Models;

namespace backend.Repositories
{
    public class EventRepository
    {
        private string _sqlConnectionString;
        public EventRepository(bool isTest = false)
        {
            if(isTest)
            {
                _sqlConnectionString = Properties.Resources.testsqlconnection;
            }
            else
            {
                _sqlConnectionString = Properties.Resources.sqlconnection;
            }
        }
        public bool DoesEventBelongToUser(int currentEventId)
        {
            bool permit;
            using(SqlConnection conn = new SqlConnection(_sqlConnectionString))
            {
                conn.Open();
                string checkEventQuery = @"select * from cn.Events where ListingId = @ListingId and UserId = @UserId";
                using (SqlCommand cmd = new SqlCommand(checkEventQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@ListingId", currentEventId);
                    cmd.Parameters.AddWithValue("@UserId", LoginRepository.CurrentUser.UserId);
                    using(SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if(reader.Read())
                        {
                            permit = Int32.Parse(reader["UserId"].ToString()) == LoginRepository.CurrentUser.UserId
                                && Int32.Parse(reader["ListingId"].ToString()) == currentEventId;
                        }
                        else 
                        {
                            permit = false;
                        }
                    }
                }

            }
            return permit;
        }

        public EventRecord GetEventById(int eventId)
        {
            try
            {
                EventRecord retrievedEvent = new EventRecord();
                using (SqlConnection conn = new SqlConnection(_sqlConnectionString))
                {
                    conn.Open();
                    string getEventQuery = @"select * from cn.Events where ListingId = @eventId";
                    using (SqlCommand cmd = new SqlCommand(getEventQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@eventId", eventId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                retrievedEvent = new EventRecord()
                                {
                                    ListingId = Int32.Parse(reader["ListingId"].ToString()),
                                    Title = reader["Title"].ToString(),
                                    Description = reader["Description"].ToString(),
                                    StartTime = DateTime.Parse(reader["StartTime"].ToString()),
                                    EndTime = DateTime.Parse(reader["EndTime"].ToString()),
                                    LocX = float.Parse(reader["LocX"].ToString()),
                                    LocY = float.Parse(reader["LocY"].ToString()),
                                    UserId = Int32.Parse(reader["UserId"].ToString())
                                };
                            }
                        }
                    }
                }
                return retrievedEvent;
            }
            catch(SqlException e)
            {
                throw new RepoException(e.Message);
            }
        }

        public List<EventRecord> GetAllEvents()
        {
            try
            {
                List<EventRecord> response = new List<EventRecord>();
                EventRecord retrievedEvent;
                using (SqlConnection conn = new SqlConnection(_sqlConnectionString))
                {
                    conn.Open();
                    string getEventsQuery = "select * from cn.Events";
                    using (SqlCommand cmd = new SqlCommand(getEventsQuery, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                retrievedEvent = new EventRecord()
                                {
                                    ListingId = Int32.Parse(reader["ListingId"].ToString()),
                                    Title = reader["Title"].ToString(),
                                    Description = reader["Description"].ToString(),
                                    StartTime = DateTime.Parse(reader["StartTime"].ToString()),
                                    EndTime = DateTime.Parse(reader["EndTime"].ToString()),
                                    LocX = float.Parse(reader["LocX"].ToString()),
                                    LocY = float.Parse(reader["LocY"].ToString()),
                                    UserId = Int32.Parse(reader["UserId"].ToString())
                                };
                                response.Add(retrievedEvent);
                            }
                        }
                    }
                }
                return response;
            }
            catch(SqlException e)
            {
                throw new RepoException(e.Message);
            }
        }

        public string PostNewEvent(EventRecord newEvent)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_sqlConnectionString))
                {
                    conn.Open();
                    string getEventQuery = @"insert into cn.Events (Title, Description, StartTime, EndTime, LocX, Locy, UserId) values
                        (@title, @description, @start, @end, @locX, @locY, @userId);";
                    using (SqlCommand cmd = new SqlCommand(getEventQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@title", newEvent.Title);
                        cmd.Parameters.AddWithValue("@description", newEvent.Description);
                        cmd.Parameters.AddWithValue("@start", newEvent.StartTime);
                        cmd.Parameters.AddWithValue("@end", newEvent.EndTime);
                        cmd.Parameters.AddWithValue("@locX", newEvent.LocX);
                        cmd.Parameters.AddWithValue("@locY", newEvent.LocY);
                        cmd.Parameters.AddWithValue("@userId", LoginRepository.CurrentUser.UserId);

                        cmd.ExecuteNonQuery();
                    }
                }
                return string.Format("Successfully created event {0}", newEvent.Title);
            }
            catch(SqlException e)
            {
                throw new RepoException(e.Message);
            }
        }

        public string UpdateEvent(EventRecord updated)
        {
            if (DoesEventBelongToUser(updated.ListingId) || LoginRepository.CurrentUser.IsAdmin)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(_sqlConnectionString))
                    {
                        conn.Open();
                        String updateEventQuery = @"update cn.Events
                        set Title = @title, Description = @description, StartTime = @start, EndTime = @end, LocX = @LocX, LocY = @LocY
                        where ListingId = @id;";
                        using (SqlCommand cmd = new SqlCommand(updateEventQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@title", updated.Title);
                            cmd.Parameters.AddWithValue("@description", updated.Description);
                            cmd.Parameters.AddWithValue("@start", updated.StartTime);
                            cmd.Parameters.AddWithValue("@end", updated.EndTime);
                            cmd.Parameters.AddWithValue("@LocX", updated.LocX);
                            cmd.Parameters.AddWithValue("@LocY", updated.LocY);
                            cmd.Parameters.AddWithValue("@id", updated.ListingId);

                            cmd.ExecuteNonQuery();
                        }
                    }
                    return string.Format("Successfully updated event {0}", updated.Title);
                }
                catch (SqlException e)
                {
                    throw new RepoException(e.Message);
                }
            }
            else 
            {
                throw new RepoException("Event does not belong to user!");
            }
        }

        public string DeleteEvent(int eventId)
        {
            if (DoesEventBelongToUser(eventId) || LoginRepository.CurrentUser.IsAdmin)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(_sqlConnectionString))
                    {
                        conn.Open();
                        string deleteEventQuery = "delete from cn.Events where ListingId = @eventId";
                        using (SqlCommand cmd = new SqlCommand(deleteEventQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@eventId", eventId);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    return string.Format("Successfully deleted event {0}", eventId);
                }
                catch (SqlException e)
                {
                    throw new RepoException(e.Message);
                }
            }
            else
            {
                throw new RepoException("Event does not belong to user!");
            }
        }

        public List<EventRecord> GetEventsByUserId(int UserId)
        {
            List<EventRecord> response = new List<EventRecord>();
            EventRecord userEvent;
            try 
            {
                using(SqlConnection conn = new SqlConnection(_sqlConnectionString))
                {
                    conn.Open();
                    string getEventByUserIdQuery = @"select * from cn.Events where UserId = @UserId;";
                    using (SqlCommand cmd = new SqlCommand(getEventByUserIdQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserId", UserId);
                        using(SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                userEvent = new EventRecord()
                                {
                                    ListingId = Int32.Parse(reader["ListingId"].ToString()),
                                    Title = reader["Title"].ToString(),
                                    Description = reader["Description"].ToString(),
                                    StartTime = DateTime.Parse(reader["StartTime"].ToString()),
                                    EndTime = DateTime.Parse(reader["EndTime"].ToString()),
                                    LocX = float.Parse(reader["LocX"].ToString()),
                                    LocY = float.Parse(reader["LocY"].ToString()),
                                    UserId = Int32.Parse(reader["UserId"].ToString())
                                };
                                response.Add(userEvent);
                            }
                        }
                    }
                }
            }
            catch(SqlException e)
            {
                throw new RepoException(e.Message);
            }
            return response;
        }

        public List<EventRecord> GetEventsByTimeRange(DateTime startTime, DateTime endTime)
        {
            List<EventRecord> response = new List<EventRecord>();
            EventRecord userEvent;
            try
            {
                if (!(startTime < endTime))
                {
                    throw new ArgumentException("Starttime must be before endtime!");
                }
                using (SqlConnection conn = new SqlConnection(_sqlConnectionString))
                {
                    conn.Open();
                    string getEventByUserIdQuery = @"select * from cn.Events where StartTime > @startTime and StartTime < @endTime;";
                    using (SqlCommand cmd = new SqlCommand(getEventByUserIdQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@startTime", startTime);
                        cmd.Parameters.AddWithValue("@endTime", endTime);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                userEvent = new EventRecord()
                                {
                                    ListingId = Int32.Parse(reader["ListingId"].ToString()),
                                    Title = reader["Title"].ToString(),
                                    Description = reader["Description"].ToString(),
                                    StartTime = DateTime.Parse(reader["StartTime"].ToString()),
                                    EndTime = DateTime.Parse(reader["EndTime"].ToString()),
                                    LocX = float.Parse(reader["LocX"].ToString()),
                                    LocY = float.Parse(reader["LocY"].ToString()),
                                    UserId = Int32.Parse(reader["UserId"].ToString())
                                };
                                response.Add(userEvent);
                            }
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                throw new RepoException(e.Message);
            }
            catch(ArgumentException e)
            {
                throw new RepoException(e.Message);
            }
            return response;
        }

        public List<EventRecord> GetEventsByRadius (float centerX, float centerY, float radius)
        {
            List<EventRecord> response = new List<EventRecord>();
            EventRecord userEvent;
            try
            {
                if (radius < 0)
                {
                    throw new ArgumentException("Radius must be positive");
                }
                using (SqlConnection conn = new SqlConnection(_sqlConnectionString))
                {
                    conn.Open();
                    string getEventByUserIdQuery = @"select * from cn.Events";
                    using (SqlCommand cmd = new SqlCommand(getEventByUserIdQuery, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                userEvent = new EventRecord()
                                {
                                    ListingId = Int32.Parse(reader["ListingId"].ToString()),
                                    Title = reader["Title"].ToString(),
                                    Description = reader["Description"].ToString(),
                                    StartTime = DateTime.Parse(reader["StartTime"].ToString()),
                                    EndTime = DateTime.Parse(reader["EndTime"].ToString()),
                                    LocX = float.Parse(reader["LocX"].ToString()),
                                    LocY = float.Parse(reader["LocY"].ToString()),
                                    UserId = Int32.Parse(reader["UserId"].ToString())
                                };
                                double distance = Math.Sqrt(
                                    Math.Pow(userEvent.LocX - centerX, 2) + Math.Pow(userEvent.LocY - centerY, 2));
                                if (distance < radius)
                                {
                                    response.Add(userEvent);
                                }
                            }
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                throw new RepoException(e.Message);
            }
            catch (ArgumentException e)
            {
                throw new RepoException(e.Message);
            }
            return response;
        }

        /*
        * The methods below are to be used only for testing, and
        * will not work on the production database
        */
        public bool ResetEvents()
        {
            if (_sqlConnectionString == Properties.Resources.testsqlconnection)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(_sqlConnectionString))
                    {
                        conn.Open();
                        string getEventQuery = @"delete from cn.Events;";
                        using (SqlCommand cmd = new SqlCommand(getEventQuery, conn))
                        {
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                catch (SqlException e)
                {
                    return false;
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool ResetAutoIncrement()
        {
            if (_sqlConnectionString == Properties.Resources.testsqlconnection)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(_sqlConnectionString))
                    {
                        conn.Open();
                        string resetIdentityQuery = @"DBCC CHECKIDENT('cn.Events', RESEED, 0)";
                        using (SqlCommand cmd = new SqlCommand(resetIdentityQuery, conn))
                        {
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                catch (SqlException e)
                {
                    return false;
                }
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
