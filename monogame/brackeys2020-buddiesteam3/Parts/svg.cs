using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using Microsoft.Xna.Framework;

namespace brackeys2020_buddiesteam3
{
	public enum LevelPieceType
	{
		Platform,
		Ground,
		Button,
		Spikes,
		BreakingPlatform,
		MovingPlatform,
		FirstCharacterStart,
		SecondCharacterStart
	}

	public class SVGLevelPiece
	{
		public Rectangle Rect;
		public LevelPieceType Type;

		public SVGLevelPiece(Rectangle rect, LevelPieceType type)
		{
			Rect = rect;
			Type = type;
		}
	}

	public class Parsing
	{
		const string svgNamespace = "{http://www.w3.org/2000/svg}";

		public static List<List<SVGLevelPiece>> Parse(string svgPath)
		{

			XDocument XD = XDocument.Load(uri: svgPath);
			XElement SVG_Element = XD.Root;

			var layerList = new List<List<SVGLevelPiece>>();
			var layers = SVG_Element.Elements(svgNamespace + "g");

			//Debug.WriteLine(layers.Count() + " layers found");

			foreach (var layer in layers)
			{

				// if (layer. ttribute("style")?.Value ?? "")
				{
					
				}

				List<SVGLevelPiece> shapeList = new List<SVGLevelPiece>();

				foreach (var element in layer.Elements())
				{
					//Debug.WriteLine("found element " + element.Name);
					if (element.Name.LocalName == "rect")
					{
						var retRectangle = new Rectangle(
							(int)(float)element.Attribute("x"),
							(int)(float)element.Attribute("y"),
							(int)(float)element.Attribute("width"),
							(int)(float)element.Attribute("height")
						);

						Debug.WriteLine("Found Rectangle "+ retRectangle);

						LevelPieceType type = LevelPieceType.Platform;
						string style = element.Attribute("style")?.Value ?? "";

						string color = element.Attribute("fill")?.Value ?? "#FFFFFF";
						string[] words = style.Split(';');
						foreach (var item in words)
						{
							if (item.Contains("fill:"))
							{
								color = item.Substring(5);
							}
						}

						// platform: 7f0000  
						// ground: 007f00 
						// spikes: bf00bf 
						// spawn zone 1: 0000ff 
						// spawn zone 2: 00ffff 
						// breaking platform: ffff00
						switch (color)
						{
							case "#7f0000":
							case "#7F0000":
								type = LevelPieceType.Platform;
								break;
							case "#007f00":
							case "#007F00":
								type = LevelPieceType.Ground;
								break;
							case "#bf00bf":
							case "#BF00BF":
								type = LevelPieceType.Spikes;
								break;
							case "#0000ff":
							case "#0000FF":
								type = LevelPieceType.FirstCharacterStart;
								break;
							case "#00ffff":
							case "#00FFFF":
								type = LevelPieceType.SecondCharacterStart;
								break;
							case "#ffff00":
							case "#FFFF00":
								type = LevelPieceType.BreakingPlatform;
								break;
							default:
								continue;
								// break;
						}

						// TODO: check outline to link buttons to triggerable environment

						shapeList.Add(new SVGLevelPiece(retRectangle, type));
					}
					// case "circle":
					// 	shapeList.Add(new Circle(
					// 		(float) element.Attribute("cx"),
					// 		(float) element.Attribute("cy"),
					// 		(float) element.Attribute("r")
					// 	));
					// case "ellipse":
					// 	shapeList.Add(new Ellipse(
					// 		(float) element.Attribute("cx"),
					// 		(float) element.Attribute("cy"),
					// 		(float) element.Attribute("rx"),
					// 		(float) element.Attribute("ry")
					// 	));

				}
				//Debug.WriteLine("shapes in layer: " + shapeList.Count);
				layerList.Add(shapeList);
			}

			return layerList;
		}
	}
}