using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LibraryLink.Models
{
    public static class DatabaseConfig
    {
        // 公开的只读属性，保存连接字段
        public static string ConnectionString { get; } = "server=localhost; Database=LibraryLinkDB; Integrated Security=True;";
    }
}