using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindNeighbors : GameManager
{
    public List<HexStruct> SelectLinearNeighbors(HexStruct originHexStruct, int range, bool includeOrigin, string selection) {

        if(DoesHexExist(originHexStruct) == false) {
            return new List<HexStruct>();
        }

        int xPos = originHexStruct.arrayPos.x;
        int zPos = originHexStruct.arrayPos.y; //Vector2 stores z position as y
        bool oddTracker = originHexStruct.odd;

        List<HexStruct> selectedHexes = new List<HexStruct>();
        
        switch (selection) {
            case "L":
                //Left
                if(!includeOrigin) {
                    xPos --;
                }
                for (int i = 0; i < range; i++) {
                    if(xPos >= 0 && xPos < hexGrid.GetLength(0) && hexGrid[xPos,zPos].interactible) {
                        selectedHexes.Add(hexGrid[xPos,zPos]);
                        xPos --;
                    }
                }
                break;

            case "R":
                //Right
                if(!includeOrigin) {
                    xPos ++;
                }
                for (int i = 0; i < range; i++) {
                    if(xPos >= 0 && xPos < hexGrid.GetLength(0) && hexGrid[xPos,zPos].interactible) {
                        selectedHexes.Add(hexGrid[xPos,zPos]);
                        xPos ++;
                    }
                }
                break;

            case "TL":
                //Top Left
                oddTracker = originHexStruct.odd;
                if(!includeOrigin && oddTracker) {
                    zPos ++;
                    oddTracker = !oddTracker;
                } else if (!includeOrigin) {
                    xPos --;
                    zPos ++;
                    oddTracker = !oddTracker;
                } 
                for (int i = 0; i < range; i++) {
                    if(xPos >= 0 && xPos < hexGrid.GetLength(0) && zPos >= 0 && zPos < hexGrid.GetLength(1) && hexGrid[xPos,zPos].interactible) {
                        if(oddTracker) {
                            selectedHexes.Add(hexGrid[xPos,zPos]);
                            zPos ++;
                            oddTracker = !oddTracker;
                        } else {
                            selectedHexes.Add(hexGrid[xPos,zPos]);
                            xPos --;
                            zPos ++;
                            oddTracker = !oddTracker;
                        }
                    }
                }
                break;

            case "TR":
                //Top Right
                oddTracker = originHexStruct.odd;
                if(!includeOrigin && oddTracker) {
                    xPos ++;
                    zPos ++;
                    oddTracker = !oddTracker;
                } else if (!includeOrigin) {
                    zPos ++;
                    oddTracker = !oddTracker;
                } 
                for (int i = 0; i < range; i++) {
                    if(xPos >= 0 && xPos < hexGrid.GetLength(0) && zPos >= 0 && zPos < hexGrid.GetLength(1) && hexGrid[xPos,zPos].interactible) {
                        if(oddTracker) {
                            selectedHexes.Add(hexGrid[xPos,zPos]);
                            xPos ++;
                            zPos ++;
                            oddTracker = !oddTracker;
                        } else {
                            selectedHexes.Add(hexGrid[xPos,zPos]);
                            zPos ++;
                            oddTracker = !oddTracker;
                        }
                    }
                }
                break;

            case "BL":
                //Bottom Left
                oddTracker = originHexStruct.odd;
                if(!includeOrigin && oddTracker) {
                    zPos --;
                    oddTracker = !oddTracker;
                } else if (!includeOrigin) {
                    xPos --;
                    zPos --;
                    oddTracker = !oddTracker;
                } 
                for (int i = 0; i < range; i++) {
                    if(xPos >= 0 && xPos < hexGrid.GetLength(0) && zPos >= 0 && zPos < hexGrid.GetLength(1) && hexGrid[xPos,zPos].interactible) {
                        if(oddTracker) {
                            selectedHexes.Add(hexGrid[xPos,zPos]);
                            zPos --;
                            oddTracker = !oddTracker;
                        } else {
                            selectedHexes.Add(hexGrid[xPos,zPos]);
                            xPos --;
                            zPos --;
                            oddTracker = !oddTracker;
                        }
                    }
                }
                break;

            case "BR":
                //Bottom Right
                oddTracker = originHexStruct.odd;
                if(!includeOrigin && oddTracker) {
                    xPos ++;
                    zPos --;
                    oddTracker = !oddTracker;
                } else if (!includeOrigin) {
                    zPos --;
                    oddTracker = !oddTracker;
                } 
                for (int i = 0; i < range; i++) {
                    if(xPos >= 0 && xPos < hexGrid.GetLength(0) && zPos >= 0 && zPos < hexGrid.GetLength(1) && hexGrid[xPos,zPos].interactible) {
                        if(oddTracker) {
                            selectedHexes.Add(hexGrid[xPos,zPos]);
                            xPos ++;
                            zPos --;
                            oddTracker = !oddTracker;
                        } else {
                            selectedHexes.Add(hexGrid[xPos,zPos]);
                            zPos --;
                            oddTracker = !oddTracker;
                        }
                    }
                }
                break;
            default:
                //Return Origin
                selectedHexes.Add(hexGrid[xPos,zPos]);
                break;
        }
        return selectedHexes;
    }


    public List<HexStruct> SelectCircularNeighbors(HexStruct originHexStruct, int range, bool includeOrigin) {

        if(DoesHexExist(originHexStruct) == false) {
            return new List<HexStruct>();
        }

        HexStruct currentHex = originHexStruct; //Hex to start with
        List<HexStruct> list = new List<HexStruct> {currentHex}; //Add to list to be evaluated
        List<HexStruct> closedList = new List<HexStruct>();
        List<HexStruct> openList = new List<HexStruct>();

        for (int i = 0; i < range; i++) {
            
            if (list.Count > 0) {

                int rotation = list.Count;

                for (int l = 0; l < rotation; l++) {
                    if(!closedList.Contains(list[l])) {                

                        openList = FindImmediateNeighbors(list[l]); 

                        for (int j = 0; j < openList.Count; j++) {
                            if(!list.Contains(openList[j]) && openList[j].interactible) {
                                list.Add(openList[j]);
                            }
                        }
                        openList.Clear();
                        closedList.Add(list[l]);
                    }
                }
            }
        }

        if(!includeOrigin) {
            list.Remove(originHexStruct);
        }

        return list;
    }

    List<HexStruct> FindImmediateNeighbors(HexStruct currentHex) {

        List<HexStruct> tempSelection = new List<HexStruct>();

        if(DoesHexExist(currentHex)) {
            List<HexStruct> TLnb = SelectLinearNeighbors(currentHex, 1, false, "TL");
            if (TLnb.Count > 0) {
                tempSelection.Add(TLnb[0]);
            }
            List<HexStruct> TRnb = SelectLinearNeighbors(currentHex, 1, false, "TR");
            if (TRnb.Count > 0) {
                tempSelection.Add(TRnb[0]);
            }
            List<HexStruct> BRnb = SelectLinearNeighbors(currentHex, 1, false, "BR");
            if (BRnb.Count > 0) {
                tempSelection.Add(BRnb[0]);
            }
            List<HexStruct> BLnb = SelectLinearNeighbors(currentHex, 1, false, "BL");
            if (BLnb.Count > 0) {
                tempSelection.Add(BLnb[0]);
            }
            List<HexStruct> Lnb = SelectLinearNeighbors(currentHex, 1, false, "L");
            if (Lnb.Count > 0) {
                tempSelection.Add(Lnb[0]);
            }
            List<HexStruct> Rnb = SelectLinearNeighbors(currentHex, 1, false, "R");
            if (Rnb.Count > 0) {
                tempSelection.Add(Rnb[0]);
            }
        }
        
        return tempSelection;
    }


    bool DoesHexExist(HexStruct hex) {
        int xPos = hex.arrayPos.x;
        int zPos = hex.arrayPos.y;

        if(xPos > 0 && xPos < hexGrid.GetLength(0) && zPos > 0 && zPos < hexGrid.GetLength(0)) {
            return true;
        } else {
            return false;
        }
    }
}
