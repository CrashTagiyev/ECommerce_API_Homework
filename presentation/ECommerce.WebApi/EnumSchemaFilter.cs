using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ECommerce.WebApi
{
	public class EnumSchemaFilter : ISchemaFilter
	{
		public void Apply(OpenApiSchema schema, SchemaFilterContext context)
		{
			if (context.Type.IsEnum)
			{
				var enumValues = Enum.GetNames(context.Type)
											.Select(name => new OpenApiInteger((int)Enum.Parse(context.Type, name)))
											.Cast<IOpenApiAny>()
											.ToList();
				schema.Enum = enumValues;
				schema.Type = "string";
				schema.Format = null;
			}
		}
	}
}
