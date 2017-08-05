namespace DaisyParser.Tests.Parser

open NUnit.Framework
open FSharp.Data

module HeadTests =
  [<Test>]
  let ``Test smoke test`` () =
    Assert.AreEqual(42,43)

module FSharpDataTests = 
  open DaisyParser.Parser.Domain
  open System

  [<Test>]
  let ``Can Parse Head`` () = 
    let testDoc =
        """<?xml version="1.0" encoding="iso-8859-1"?>
        <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN"
           "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
        <html xmlns="http://www.w3.org/1999/xhtml" xml:lang="en" lang="en">
        <head>
          <title>Economics</title>
          <meta http-equiv="Content-type" content='text/html; charset="iso-8859-1"' />
          <meta name="dc:title" content="Economics" />
          <meta name="dc:creator" content="Richard G. Lipsey" />
          <meta name="dc:creator" content="Paul N. Courant" />
          <meta name="dc:creator" content="Douglas D. Purvis" />
          <meta name="dc:creator" content="Peter O. Steiner" />
          <meta name="dc:date" content="2000-09-05" scheme="yyyy-mm-dd" />
          <meta name="dc:format" content="Daisy 2.02" />
          <meta name="dc:identifier" content="DTB00345" />
          <meta name="dc:language" content="EN" scheme="ISO 639" />
          <meta name="dc:publisher" content="TPB" />
          <meta name="dc:source" content="0-065-01022-1" scheme="ISBN" />
          <meta name="dc:subject" content="Qb" />
          <meta name="ncc:sourceDate" content="1993" scheme="yyyy" />
          <meta name="ncc:sourceEdition" content="1" />
          <meta name="ncc:sourcePublisher" content="Harper Collins" />
          <meta name="ncc:charset" content="iso-8859-1" />
          <meta name="ncc:generator" content="LpStudioGen 1.6" />
          <meta name="ncc:narrator" content="Timothy Ocklind" />
          <meta name="ncc:tocItems" content="1024" />
          <meta name="ncc:totalTime" content="91:27:21" scheme="hh:mm:ss" />
          <meta name="ncc:pageNormal" content="881" />
          <meta name="ncc:maxPageNormal" content="881" />
          <meta name="ncc:pageFront" content="27" />
          <meta name="ncc:pageSpecial" content="45" />
          <meta name="ncc:prodNotes" content="0" />
          <meta name="ncc:footnotes" content="0" />
          <meta name="ncc:sidebars" content="0" />
          <meta name="ncc:setInfo" content="1 of 3" />
          <meta name="ncc:depth" content="4" />
          <meta name="ncc:kByteSize" content="1530000" />
          <meta name="ncc:multimediaType" content="audioNCC" />
          <meta name="ncc:files" content="97" />
          <meta name="prod:recLocation" content="Studio 2" />
          <meta name="prod:recEngineer" content="J Klein" />
          <bad name="prod:recEngineer" content="J Klein" />
        </head>"""

    let htmlDoc = HtmlDocument.Parse(testDoc)
    let headChildren = 
      htmlDoc.Descendants "head" 
      |> Seq.head
      |> HtmlNode.elementsNamed ["title" ; "meta"]
    let title = 
      headChildren
      |> Seq.find (fun x -> x.HasName "title")
      |> HtmlNodeExtensions.DirectInnerText
    
    let metadata = 
      headChildren
      |> Seq.filter (HtmlNode.hasName "meta")
      |> Seq.map (fun x -> 
        (HtmlNode.attributeValue "name" x, HtmlNode.attributeValue "content" x))
      
    let getValueFromTuple (key: string) =
      metadata
      |> Seq.find (fun (k, _) -> k.ToLower() = key.ToLower())
      |> snd

    { MetaData.Creator = getValueFromTuple "dc:creator"
      Date = DateTime.Now
      Format = getValueFromTuple "dc:format"
      Publisher = getValueFromTuple "dc:publisher"
      Title = getValueFromTuple "dc:title"
      Charset = getValueFromTuple "ncc:charset"
      PageFront = getValueFromTuple "ncc:pageFront" |> int
      PageNormal = getValueFromTuple "ncc:pagenormal" |> int
      PageSpecial = getValueFromTuple "ncc:pageSpecial" |> int
      TocItems = getValueFromTuple "ncc:tocItems" |> int
      TotalTime = TimeSpan(getValueFromTuple "ncc:totaltime" |> int64)
      OptionalMetaData = [] }
    //()
    


