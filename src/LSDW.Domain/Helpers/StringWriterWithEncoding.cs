using System.Text;

namespace LSDW.Domain.Helpers;

/// <summary>
/// Overrides the base <see cref="StringWriter"/> class to accept a different character encoding type.
/// </summary>
internal sealed class StringWriterWithEncoding : StringWriter
{
	private readonly Encoding _encoding = default!;

	/// <summary>
	/// Overrides the default encoding type (UTF-16).
	/// </summary>
	public override Encoding Encoding
		=> _encoding ?? base.Encoding;

	/// <summary>
	/// Initializes a new instance of the <see cref="StringWriterWithEncoding"/> class.
	/// </summary>
	public StringWriterWithEncoding()
	{ }

	/// <summary>
	/// Initializes a new instance of the <see cref="StringWriterWithEncoding"/> class.
	/// </summary>
	/// <param name="encoding">The character encoding type</param>
	public StringWriterWithEncoding(Encoding encoding)
		=> _encoding = encoding;
}
