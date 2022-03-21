// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

//All of these are for the json parser to parse it as it doesn't parse static members
[assembly: SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "<Pending>", Scope = "member", Target = "~P:Atlas.Common.ArticleComponents.FormattedText.ComponentName")]
[assembly: SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "<Pending>", Scope = "member", Target = "~P:Atlas.Common.ArticleComponents.Section.ComponentName")]
[assembly: SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "<Pending>", Scope = "member", Target = "~P:Atlas.Common.ArticleComponents.SectionComponents.BulletList.ComponentName")]
[assembly: SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "<Pending>", Scope = "member", Target = "~P:Atlas.Common.ArticleComponents.SectionComponents.ImageGrid.ComponentName")]
[assembly: SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "<Pending>", Scope = "member", Target = "~P:Atlas.Common.ArticleComponents.SectionComponents.Paragraph.ComponentName")]
[assembly: SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "<Pending>", Scope = "member", Target = "~P:Atlas.Common.ArticleComponents.SectionComponents.Table.ComponentName")]
