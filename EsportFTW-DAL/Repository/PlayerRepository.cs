﻿using System.Data;
using EsportFTW_DAL.Interface;
using EsportFTW_DAL.Model;
using Oracle.ManagedDataAccess.Client;

namespace EsportFTW_DAL.Repository
{
    internal class PlayerRepository : Database, IRepository<Player, bool>
    {
        public IEnumerable<Player> Get()
        {
            return ExecuteReaderQuery(AllPlayerDataQuery, MapPlayer);
        }

        public Player Get(int id)
        {
            const string query = AllPlayerDataQuery + " WHERE p.player_id = :id";
            var parameter = new OracleParameter(":id", OracleDbType.Int32) { Value = id };
            return ExecuteReaderQuery(query, MapPlayer, new[] { parameter }).FirstOrDefault()!;
        }

        public bool Add(Player entity)
        {
            const string query = "INSERT INTO Player (Player_ID, Player_Name, Player_Email, Player_Password, Player_Picture, Player_JoinDate, Player_Play_Hours, Player_Salary, Player_DOB) " +
                                 "VALUES (seq_player_id.NEXTVAL, :name, :email, :password, :picture, :joinDate, :playHours, :salary, :dob)";

            var parameters = new[]
            {
                new OracleParameter(":name", OracleDbType.Varchar2) { Value = entity.Name },
                new OracleParameter(":email", OracleDbType.Varchar2) { Value = entity.Email },
                new OracleParameter(":password", OracleDbType.Varchar2) { Value = entity.Password },
                new OracleParameter(":picture", OracleDbType.Varchar2) { Value = entity.Picture },
                new OracleParameter(":joinDate", OracleDbType.Date) { Value = entity.JoinDate },
                new OracleParameter(":playHours", OracleDbType.Double) { Value = entity.PlayHours },
                new OracleParameter(":salary", OracleDbType.Decimal) { Value = entity.Salary },
                new OracleParameter(":dob", OracleDbType.Date) { Value = entity.DOB }
            };

            var rowsAffected = ExecuteNonQuery(query, parameters);
            return rowsAffected > 0;
        }

        public bool Update(Player entity)
        {
            const string query = "UPDATE Player SET player_name = :name, player_email = :email, player_password = :password, player_picture = :picture, " +
                                 "player_joindate = :joinDate, player_salary = :salary, player_play_hours = :playHours, player_dob = :dob WHERE player_id = :id";

            var parameters = new[]
            {
                new OracleParameter(":name", OracleDbType.Varchar2) { Value = entity.Name },
                new OracleParameter(":email", OracleDbType.Varchar2) { Value = entity.Email },
                new OracleParameter(":password", OracleDbType.Varchar2) { Value = entity.Password },
                new OracleParameter(":picture", OracleDbType.Varchar2) { Value = entity.Picture },
                new OracleParameter(":joinDate", OracleDbType.Date) { Value = entity.JoinDate },
                new OracleParameter(":salary", OracleDbType.Decimal) { Value = entity.Salary },
                new OracleParameter(":playHours", OracleDbType.Int32) { Value = entity.PlayHours },
                new OracleParameter(":dob", OracleDbType.Date) { Value = entity.DOB },
                new OracleParameter(":id", OracleDbType.Int32) { Value = entity.Id }
            };

            var rowsAffected = ExecuteNonQuery(query, parameters.ToArray());
            return rowsAffected > 0;
        }

        public bool Delete(int id)
        {
            const string query = "DELETE FROM player WHERE player_id = :id";
            var parameter = new OracleParameter(":id", OracleDbType.Int32) { Value = id };

            var rowsAffected = ExecuteNonQuery(query, new[] { parameter });
            return rowsAffected > 0;
        }

        public bool EmailExists(string email)
        {
            const string query = "SELECT COUNT(*) FROM player WHERE player_email = :email";
            var parameter = new OracleParameter(":email", OracleDbType.Varchar2) { Value = email };

            var count = ExecuteScalarQuery(query, new[] { parameter });

            return count > 0;
        }


        private static Player MapPlayer(IDataRecord reader)
        {
            var player = new Player
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                Email = reader.GetString(2),
                Password = reader.GetString(3),
                Picture = reader.GetString(4),
                JoinDate = reader.GetDateTime(5),
                PlayHours = reader.GetInt32(6),
                Salary = reader.GetDecimal(7),
                DOB = reader.GetDateTime(8)
            };

            if (!reader.IsDBNull(9))
            {
                player.Address = new PlayerAddress
                {
                    ID = reader.GetInt32(9),
                    Country = reader.GetString(10),
                    City = reader.GetString(11),
                    Street = reader.GetString(12),
                    ZipCode = reader.GetString(13),
                };
            }

            if (!reader.IsDBNull(14))
            {
                player.SocialLinks = new PlayerSocialLink
                {
                    ID = reader.GetInt32(14),
                    FacebookLink = reader.GetString(15),
                    InstagramLink = reader.GetString(16),
                    TwitterLink = reader.GetString(17),
                    YoutubeLink = reader.GetString(18),
                };
            }

            if (!reader.IsDBNull(19))
            {
                player.PlayerPhones = new List<PlayerPhone>
                {
                    new PlayerPhone
                    {
                        ID = reader.GetInt32(19),
                        Phone = reader.GetString(20),
                    },
                };
            }

            if (!reader.IsDBNull(21))
            {
                player.Team = new Team
                {
                    ID = reader.GetInt32(21),
                    Name = reader.GetString(22),
                };
            }

            return player;
        }


        private const string AllPlayerDataQuery = "select * from player_detail_view";

    }
}
