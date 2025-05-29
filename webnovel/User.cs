﻿namespace bookservice {
    public enum UserRole {
        GUEST,  // Renamed from WN_GUEST
        READER, // Renamed from WN_READER
        ADMIN   // Renamed from WN_WRITER
    }
    public class User {
        private int id;
        private string login;
        private string hashedPassword;
        private bool isAdmin; // Renamed from isWriter
        private UserRole role;
        public User() {
            role = UserRole.GUEST; // Default role
            id = 0;
            login = "";
            hashedPassword = "";
            isAdmin = false; // Renamed from isWriter
        }
        public int Id {
            get { return id; }
            set { id = value; }
        }

        public string Login {
            get { return login; }
            set { login = value; }
        }

        public string HashedPassword {
            get { return hashedPassword; }
            set { hashedPassword = value; }
        }

        public bool IsAdmin { // Renamed from IsWriter
            get { return isAdmin; }
            set { isAdmin = value; }
        }
        public UserRole Role {
            get { return role; }
            set { role = value; }
        }
    }
}
