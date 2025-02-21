﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Maui
{
	public static class ITextInputExtensions
	{
		public static void UpdateText(this ITextInput textInput, string? text)
		{
			// Even though <null> is technically different to "", it has no
			// functional difference to apps. Thus, hide it.
			var mauiText = textInput.Text ?? string.Empty;
			var platformText = text ?? string.Empty;
			if (mauiText != platformText)
				textInput.Text = platformText;
		}

#if __IOS__
		public static bool TextWithinMaxLength(this ITextInput textInput, string? text, Foundation.NSRange range, string replacementString)
		{
			var currLength = text?.Length ?? 0;

			// fix a crash on undo
			if (range.Length + range.Location > currLength)
				return false;

			if (textInput.MaxLength < 0)
				return true;

			var addLength = replacementString?.Length ?? 0;
			var remLength = range.Length;

			var newLength = currLength + addLength - remLength;

			return newLength <= textInput.MaxLength;
		}
#endif

#if __ANDROID__
		public static void UpdateText(this ITextInput textInput, Android.Text.TextChangedEventArgs e)
		{
			if (e.BeforeCount == 0 && e.AfterCount == 0)
				return;

			if (e.Text is Java.Lang.ICharSequence cs)
				textInput.UpdateText(cs.ToString());
			else if (e.Text != null)
				textInput.UpdateText(String.Concat(e.Text));
			else
				textInput.UpdateText((string?)null);
		}
#endif
	}
}
