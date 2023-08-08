using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Implementations.Queries.Users.Dtos;
using DataAccess.Entities;

namespace Business.Implementations.Queries.Users;

    public class GetUserByUsernameQuery
    {
        public string username { get; }

        public GetUserByUsernameQuery(string userame)
        {
            this.username = username;
        }
    }

