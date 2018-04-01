using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace SimpleComputer.Models
{
	public class PostEntity : TableEntity
	{
		public DateTime PostDate { get; set; }
		public string Title { get; set; }
		public string Content { get; set; }
		public string ImagePath { get; set; }

		public PostEntity() {}

		public PostEntity(string title, string content)
		{
			PostDate = DateTime.Now;
			PartitionKey = DateTime.Now.ToString("yyyyMMdd");
			RowKey = DateTime.Now.Ticks.ToString();
			Title = title;
			Content = content;
		}
	}
}