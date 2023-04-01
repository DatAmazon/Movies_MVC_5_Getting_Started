using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;


namespace MvcMovie.Models
{
    public class Movie
    {
        public int ID { get; set; }
        [Required, StringLength(60, MinimumLength = 3)]
        public string Title { get; set; }

        [Display(Name = "Release Date"), DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ReleaseDate { get; set; }

        [Required, StringLength(30)]
        public string Genre { get; set; }

        [Range(1, 100), DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [Required, StringLength(5)]
        public string Rating { get; set; }
    }

    //Lớp MovieDBContext mở rộng từ lớp DbContext của Entity Framework.
    //Lớp MovieDBContext đại diện cho một obj kết nối đến DB, cung cấp các phương thức để tương tác với các tbl.
    public class MovieDBContext : DbContext
    {
        //Lớp MovieDBContext có thuộc tính Movies được định nghĩa với kiểu dữ liệu DbSet<Movie>.
        //Thuộc tính Movies đại diện cho bảng Movies trong cơ sở dữ liệu và cung cấp các phương thức để truy xuất và thay đổi dữ liệu trong bảng.
        public DbSet<Movie> Movies { get; set; }
    }
    //Để sử dụng lớp MovieDBContext, tạo một đối tượng mới của nó và sử dụng các phương thức có sẵn để thực hiện các thao tác với cơ sở dữ liệu: truy vấn dữ liệu, thêm mới, cập nhật hoặc xóa dữ liệu.
}