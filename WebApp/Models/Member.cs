﻿using System;

namespace BookLibraryManagementSystem.Models
{
    public class Member
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int YearOfBirth { get; set; }
        public string Address { get; set; }
        public DateTime DateOfRegistration { get; set; }
    }
}
