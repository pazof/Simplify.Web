﻿using System;

namespace Simplify.Web.Responses
{
	/// <summary>
	/// Provides Http file response (send file to response)
	/// </summary>
	public class File : ControllerResponse
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="File"/> class.
		/// </summary>
		/// <param name="outputFileName">The name of the file.</param>
		/// <param name="contentType">Type of the content.</param>
		/// <param name="data">The data of the file.</param>
		public File(string outputFileName, string contentType, byte[] data)
		{
			if (outputFileName == null) throw new ArgumentNullException(nameof(outputFileName));
			if (contentType == null) throw new ArgumentNullException(nameof(contentType));
			if (data == null) throw new ArgumentNullException(nameof(data));

			OutputFileName = outputFileName;
			ContentType = contentType;
			Data = data;
		}

		/// <summary>
		/// Gets the name of the output file.
		/// </summary>
		/// <value>
		/// The name of the output file.
		/// </value>
		public string OutputFileName { get; }

		/// <summary>
		/// Gets the type of the content.
		/// </summary>
		/// <value>
		/// The type of the content.
		/// </value>
		public string ContentType { get; }

		/// <summary>
		/// Gets the data.
		/// </summary>
		/// <value>
		/// The data.
		/// </value>
		public byte[] Data { get; }

		/// <summary>
		/// Processes this response
		/// </summary>
		public override ControllerResponseResult Process()
		{
			Context.Response.Headers.Append("Content-Disposition", "attachment; filename=" + OutputFileName);
			Context.Response.ContentType = ContentType;
			Context.Response.Write(Data);

			return ControllerResponseResult.RawOutput;
		}
	}
}