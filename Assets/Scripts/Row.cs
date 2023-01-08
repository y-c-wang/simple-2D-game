using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Row : MonoBehaviour
{
  public Block[] blocks { get; private set; }

  void Awake()
  {
    blocks = GetComponentsInChildren<Block>();
  }
}
