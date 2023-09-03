using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TwseApp.API.Models
{
    public class Todo
    {
        public Guid Id { get; set; }

        public string Description { get; set; }   
        
        public bool IsCompleted { get; set; }

        public DateTime CompletedDate { get; set; }
    }
}
