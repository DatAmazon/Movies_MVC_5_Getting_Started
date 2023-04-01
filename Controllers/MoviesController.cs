using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Net;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcMovie.Models;

namespace MvcMovie.Controllers
{
    public class MoviesController : Controller
    {
        //MovieDBContext tạo đtg kết nối DB, cung cấp các phương thức để tương tác với các tbl.
        private MovieDBContext db = new MovieDBContext();

        // GET: Movies
        //K cần HttpPost vì hđ không thay đổi trạng thái của ứng dụng, chỉ lọc dữ liệu.
        public ActionResult Index(string movieGenre, string searchString)
        {
            var GenreLst = new List<string>();
            var GenreQry = from d in db.Movies
                           orderby d.Genre
                           select d.Genre;

            //AddRange được sử dụng để thêm các phần tử của GenreQry vào GenreLst
            //Distinct() loại bỏ các giá trị trùng lặp
            GenreLst.AddRange(GenreQry.Distinct());
            ViewBag.movieGenre = new SelectList(GenreLst);


            var movies = from m in db.Movies
                         select m;
            if (!String.IsNullOrEmpty(movieGenre))
            {
                movies = movies.Where(x => x.Genre == movieGenre);
            }

            //kiểm tra xem SearchString có giá trị null hoặc chuỗi rỗng hay không.
            if (!String.IsNullOrEmpty(searchString))
            {
                movies = movies.Where(s => s.Title.Contains(searchString));
            }
            return View(movies);

            //obj db truy xuất đến thuộc tính bảng Movies, hiện list
            //return View(db.Movies.ToList());
        }

        // GET: Movies/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                //trả về một đối tượng Http Status Code (Mã trạng thái Http) với giá trị "BadRequest" (yêu cầu không hợp lệ).
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //obj db truy xuất đến thuộc tính bảng Movies, tìm id
            Movie movie = db.Movies.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }

        // GET: Movies/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Movies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.


        //chỉ được phép xử lý các yêu cầu gửi đến bằng phương thức POST, bỏ qua nếu yêu cầu được gửi đến bằng phương thức GET.
        [HttpPost]
        //bảo vệ trang web tránh khỏi tấn công Cross-Site Request Forgery(CSRF)
        [ValidateAntiForgeryToken]


        //Tránh việc toàn bộ dữ liệu được gửi lên từ form cho vào đối tượng Movie, sd attribute [Bind] chỉ định các thuộc tính cụ thể của đối tượng Movie mà ta muốn lấy từ form
        //Bind: chỉ những thuộc tính ID, Title, ReleaseDate, Genre và Price của đối tượng Movie sẽ được áp dụng dữ liệu từ form gửi lên
        public ActionResult Create([Bind(Include = "ID,Title,ReleaseDate,Genre,Price, Rating")] Movie movie)
        {
            //ModelState là obj chứa thông tin về các thuộc tính và giá trị của một đối tượng  khi được gửi lên từ form. Nó được sử dụng để kiểm tra tính hợp lệ của các giá trị được gửi lên từ form.
            //ModelState.IsValid là thuộc tính boolean trả về true nếu tất cả các giá trị của ModelState đều hợp lệ (valid), còn lại là false
            if (ModelState.IsValid)
            {
                db.Movies.Add(movie);
                db.SaveChanges();
                return RedirectToAction("Index");//chuyển tới trang index
            }

            return View(movie);
        }

        // GET: Movies/Edit/5
        //The HttpGet Edit method takes the movie ID parameter
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //obj db truy xuất đến thuộc tính bảng Movies, tìm id
            Movie movie = db.Movies.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Title,ReleaseDate,Genre,Price,Rating")] Movie movie)
        {
            if (ModelState.IsValid)
            {
                //EntityState.Modified là trạng thái của obj Entity Framework, sd chỉ định đối tượng đã bị thay đổi so với dữ liệu trong DB. Khi obj này được lưu lại, Entity Framework sẽ tự động tạo ra câu truy vấn SQL để cập nhật dữ liệu trong DB
                //Entry được sử dụng để truy cập đến một đối tượng của lớp Entity Framework tương ứng với DB.
                //Câu dưới, obj movie được truy cập thông qua phương thức Entry(movie) và đánh dấu là đã bị thay đổi bằng cách gán giá trị EntityState.Modified cho thuộc tính State. Method SaveChanges() được gọi để lưu các thay đổi vào DB.
                db.Entry(movie).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(movie);
        }

        // GET: Movies/Delete/5
        // phương thức HttpGet Delete không xóa movie đã chỉ định, nó trả về view của movie mà ta có thể gửi (HttpPost) yêu cầu xóa. Thực hiện thao tác xóa để đáp ứng yêu cầu GET hoặc bất kỳ thao tác nào khác làm thay đổi dữ liệu sẽ mở ra một lỗ hổng bảo mật.
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //obj db truy xuất đến thuộc tính bảng Movies, tìm id
            Movie movie = db.Movies.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }

        // POST: Movies/Delete/5
        //yêu cầu HTTP POST được gửi đến server với tên hành động là "Delete", phương thức được gắn thuộc tính [HttpPost, ActionName("Delete")] sẽ được thực hiện
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Movie movie = db.Movies.Find(id);
            db.Movies.Remove(movie);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            //disposing là một tham số đầu vào của phương thức Dispose
            //disposing giải phóng tài nguyên và thực hiện các hành động dọn dẹp khi đối tượng không còn được sử dụng nữa
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
