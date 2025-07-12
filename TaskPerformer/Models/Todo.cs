using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskPerformer.Models
{
    public class Todo
    {
        
        public int Id { get; set; }
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }
        public string Discription { get; set; }
        public bool IsCompleted { get; set; }  
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        [Required(ErrorMessage = "Due date is required")]
        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; } = DateTime.Today;
        public DateTime? CompletedDate { get; set; }

        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }

    }
    public class DashboardViewModel
    {
        public int TotalTasks { get; set; }
        public int CompletedTasks { get; set; }
        public int PendingTasks { get; set; }
        public List<Todo> TasksDueToday { get; set; } = new List<Todo>();
    }
}
    