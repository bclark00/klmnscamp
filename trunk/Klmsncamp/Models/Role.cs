﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Klmsncamp.Models
{
    public class Role
    {
        public int RoleID { get; set; }
        public string Description { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}