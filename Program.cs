using System;
using AutoMapper;

namespace AutoMapperRepro
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			// Arrange
			var mapper = new MapperConfiguration(config =>
			{
				// If you comment this out, everything works.
				config.AddConditionalObjectMapper();

				config.CreateMap<Input, Foo>()
					.ForMember(x => x.Bar, c => c.ResolveUsing<Resolver>());
			}).CreateMapper();

			var input = new Input
			{
				Name = "foo"
			};

			// Act
			var foo = mapper.Map<Foo>(input);

			// Assert
			if (foo.Bar.Name != input.Name)
			{
				throw new Exception($"foo.Bar.Name != input.Name");
			}
		}
	}

	internal class Foo
	{
		public Bar Bar { get; set; }
	}

	internal class Bar
	{
		public string Name { get; set; }
	}

	internal class Input
	{
		public string Name { get; set; }
	}

	internal class Resolver : IValueResolver<Input, Foo, Bar>
	{
		public Bar Resolve(
			Input source, Foo destination, Bar bar, ResolutionContext context)
		{
			bar = new Bar()
			{
				Name = source.Name
			};
			return bar;
		}
	}
}
