IpCompareUtility is designed to compare the ips. 
Using the regular expression it splits the ip string into 4 parts, 
converts these parts into numbers and compares these parts.
For example, 123.22.33.44 is equal to 123.022.033.44.

Beside that you can compare the ip ranges in the same manner.
For example 11.22.33.44/55 is the ip range.
I.e. it covers the ips from 11.22.33.44 to 11.22.33.55.
So, this range is equal to the 11.22.33.44\055.

To change the splitter ( \ or / symbol) change corresponding
regular expression - 
regExToSplitRange = @"^\b((?:\d{1,3}\.){3}(?:\d{1,3}){1})(?:(?:\\|\/)(\d{1,3})){0,1}\b" ,
namely, the (?:\\|\/)part.


