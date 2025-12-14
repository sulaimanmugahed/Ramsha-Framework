using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Ramsha.Common.Domain;
using Ramsha.EntityFrameworkCore;

namespace SimpleWebApiTemplate.Data;

[ConnectionString("Default")]
public class SimpleWebApiTemplateDbContext(DbContextOptions<SimpleWebApiTemplateDbContext> options)
: RamshaEFDbContext<SimpleWebApiTemplateDbContext>(options)
{

}
