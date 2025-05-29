namespace bookservice {
    public enum UserRole {
        WN_GUEST,
        WN_READER,
        WN_WRITER
    }
    public class User {
        private int id;
        private string login;
        private string hashedPassword;
        private bool isWriter;
        private UserRole role;
        public User() {
            role = UserRole.WN_GUEST;
            id = 0;
            login = "";
            hashedPassword = "";
            isWriter = false;
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

        public bool IsWriter {
            get { return isWriter; }
            set { isWriter = value; }
        }
        public UserRole Role {
            get { return role; }
            set { role = value; }
        }
    }
}