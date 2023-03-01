using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Chat.DataBase;
using System.Collections;

namespace Chat.DataBase
{
    public static class DataAccess
    {
        public delegate void RefreshListDelegate();
        public static event RefreshListDelegate RefreshListEvent;

        public static List<Employee> GetEmployees() => ChatDaniilEntities.GetContext().Employees.ToList(); 
        public static List<Department> GetDepartments() => ChatDaniilEntities.GetContext().Departments.ToList(); 

        public static List<Chatroom> GetChatrooms() => ChatDaniilEntities.GetContext().Chatrooms.ToList();

        public static Employee GetEmployee(string username, string password)
        {
            var encryptedPassword = ComputeStringToSha256Hash(password);
            return ChatDaniilEntities.GetContext().Employees.FirstOrDefault(x => x.Name == username && x.Password == encryptedPassword);
        }

        internal static void SaveChatroom(Chatroom chatroom)
        {
            if (chatroom.Id == 0)
                ChatDaniilEntities.GetContext().Chatrooms.Add(chatroom);

            ChatDaniilEntities.GetContext().SaveChanges();
            RefreshListEvent?.Invoke();
        }

        public static List<ChatMessage> GetChatMessages() => ChatDaniilEntities.GetContext().ChatMessages.ToList();

        public static void LeaveChat(Employee employee, Chatroom chatroom)
        {
            var employeeChat = ChatDaniilEntities.GetContext().EmployeeChats
                .FirstOrDefault(x => x.EmployeeId == employee.Id && x.ChatId == chatroom.Id);
            if (employeeChat != null)
                ChatDaniilEntities.GetContext().EmployeeChats.Remove(employeeChat);

            ChatDaniilEntities.GetContext().SaveChanges();
            RefreshListEvent?.Invoke();
        }

        public static List<ChatMessage> GetChatMessages(Chatroom chatroom)
        {
            return GetChatMessages().FindAll(x => x.Chatroom == chatroom);
        }

        public static void SaveChatMessage(ChatMessage message)
        {
            if (message.Id == 0)
                ChatDaniilEntities.GetContext().ChatMessages.Add(message);

            ChatDaniilEntities.GetContext().SaveChanges();
            RefreshListEvent?.Invoke();
        }


        static string ComputeStringToSha256Hash(string plainText)
        {
            // Create a SHA256 hash from string   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // Computing Hash - returns here byte array
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(plainText));

                // now convert byte array to a string   
                StringBuilder stringbuilder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    stringbuilder.Append(bytes[i].ToString("x2"));
                }
                return stringbuilder.ToString();
            }
        }
    }
}
