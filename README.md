# Markdig.BadHeadersMarkdownExtension
A Markdig Extension for handling bad headers without a space after the "#"

What does that mean? Your markdown can have inproper headers and proper headers together and be properly parsed.
It means this
```csharp
"# Title\nsome body text and *such*\n## Subtitle with other words\n";
```
and this
```csharp
"#Title\nsome body text and *such*\n##Subtitle with other words\n";
```
both become this
```csharp
"<h1>Title</h1>\n<p>some body text and <em>such</em></p>\n<h2>Subtitle with other words</h2>\n"
```


## Installation

Use NuGet!
https://www.nuget.org/packages/MarkdigBadHeaders

```
Install-Package MarkdigBadHeaders
```

## Usage

Add it to your pipeline that you use to parse:

```csharp
var pipelineBuilder = new MarkdownPipelineBuilder();
pipelineBuilder = MarkdownExtensions.Use<BadHeadersExtension>(pipelineBuilder);
var pipeline = pipelineBuilder.Build();
var html = Markdown.ToHtml(BAD_HEADER_MARKDOWN, _pipeline);
```

## Markdig Shoutout

Markdig is the latest and greatest of .NET Markdown processors. Without them, this wouldn't be possible.
https://github.com/lunet-io/markdig
