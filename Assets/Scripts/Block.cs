using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Block : MonoBehaviour
{
  public Image image;

  public string chess { get; set; }

  void Awake()
  {
    this.image.GetComponent<Image>().color = new Color32(0, 0, 0, 0);
    this.chess = "none";
  }

  public void setChess(string target)
  {
    switch (target)
    {
      case "black":
        this.image.GetComponent<Image>().color = new Color32(0, 0, 0, 255);
        this.chess = target;
        break;
      case "white":
        this.image.GetComponent<Image>().color = new Color32(255, 255, 225, 255);
        this.chess = target;
        break;
      case "tmp-black":
        this.image.GetComponent<Image>().color = new Color32(0, 0, 0, 127);
        this.chess = target;
        break;
      case "tmp-white":
        this.image.GetComponent<Image>().color = new Color32(255, 255, 225, 127);
        this.chess = target;
        break;
      case "none":
        this.image.GetComponent<Image>().color = new Color32(0, 0, 0, 0);
        this.chess = target;
        break;

      default:
        break;

    }
  }
}
