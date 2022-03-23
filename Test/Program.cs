// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");
Console.WriteLine();

Igtampe.BasicLogger.Logger N = new Igtampe.BasicLogger.BasicLogger(Igtampe.BasicLogger.LogSeverity.DEBUG);
Atlas.Common.Article.GlobalLogger = N;

string FullTest = @">SIDEBAR
This should be a sidebar with title SIDEBAR.

[IMG | This is the alt text | https://avatars.githubusercontent.com/u/49919240 | This is the description of the image above, which is Chopo]

|| Summary
||
| Name | Chopo |
| Occupation | Software Dev |
| Age | 20 |
| Delivery | Digiorno |

>
This should be a description that goes under the title and before the title, defined by the title of this encyclopedia entry

=Section
==Subsection
===Sub-Subsection
====And So on
=====And So Forth
I am the text from ""and so forth"". **This text should be bold**, *This text should be italicized* ***This text should be both bold and italicized***, and _***This text should be italicized, bold, and underlined***__. [This Text | Text] should link to a wiki page called ""Text"". [This Text | https://www.google.com] should link to google.
This is a second paragraph.Here nothing is tested, beyond the fact that this is a separate paragraph.

[IMG | This is the alt text | https://avatars.githubusercontent.com/u/49919240 | Another Chopo, oh god | LEFT]
Oh my god there's a chopo on the left

[IMG | This is the alt text | https://avatars.githubusercontent.com/u/49919240 | boo | RIGHT]
AH! Another chopo! This time on the right! Que locura!\nThis time with multiple lines here on the side!

|I|The Chopo Army Image Grid Grid
[IMG | This is the alt text | https://avatars.githubusercontent.com/u/49919240 | HA HA HA | 2]
[IMG | This is the alt text | https://avatars.githubusercontent.com/u/49919240 | HA HA HA | 2]
[IMG | This is the alt text | https://avatars.githubusercontent.com/u/49919240 | HA HA HA | 2]
[IMG | This is the alt text | https://avatars.githubusercontent.com/u/49919240 | HA HA HA | 2]
[IMG | This is the alt text | https://avatars.githubusercontent.com/u/49919240 | HA HA HA | 2]
[IMG | This is the alt text | https://avatars.githubusercontent.com/u/49919240 | HA HA HA | 2]

OH NO.AAAAAAAAAAAAAAAAAAAAAAAAAAAAA

==Subsection dos
This should be another subsection

The following is a list of things I care about:

- I
    - Don't
        - Care
        - About
    - Anything
- Joseph

This should not be in the list

=Section dos
This should be another primary section

Here's my top ten picks for this year

# Uno
# dos
# tres
    # A note, tres means three (spanish)
        # Tres Cosas
    # not very (french)
        # Tres grand
# cuatro
# cinco
# seis
# me cance.";

string SmallTest = @"

>Chopo
This is a test Sidebar **with a little bold text**
>

This is some more text

=Section 1
This is a section.

==Section 2
Also a section,

";

Atlas.Common.Article A = new() {
    Text = FullTest
};