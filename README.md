List files by timestamp, regardless of directory depth, i.e. all files in the current or specified directory tree are listed in order from oldest to newest, showing timestamp, size and full pathname.

The timestamp column is color-coded:

|Color|Age|
|---|---|
|Blue|Older than a year|
|Cyan|In the last year|
|Green|In the last week|
|Yellow|In the last day|
|Red|In the last hour|

By default, skips names beginning with `.` (unless they were explicitly specified on the command line). The `-a` option overrides this.
