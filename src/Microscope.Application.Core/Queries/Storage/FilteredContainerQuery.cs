using System;
using System.Collections.Generic;
using MediatR;

namespace Microscope.Application.Features.Queries.Storage
{
    public class FilteredContainerQuery : IRequest<IEnumerable<FilteredContainerQuery>>
    {
        
    }

    public class FilteredContainerQueryResult 
    {
        public string Name { get; set; }
    }
}
