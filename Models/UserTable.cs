using System;
using System.ComponentModel.DataAnnotations;

namespace ELearning.Models
{
	public class UserTable
	{
		[Key]
		public int Id { get; set; }
		public string? Username { get; set; }
		public string? Password { get; set; }
	}
}

