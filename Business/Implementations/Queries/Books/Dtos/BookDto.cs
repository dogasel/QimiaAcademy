﻿using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Business.Implementations.Queries.Books.Dtos;

public class BookDto 
{
   
    public string Title { get; set; }
    public string Author { get; set; }
    public long RequestId { get; set; }
    public DateTime UpdateDate { get; set; }
    public BookStatus Status { get; set; }
    public DateTime CreationDate { get; set; }
}

