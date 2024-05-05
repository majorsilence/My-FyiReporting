//GJL 20080520 - Limits input to an input field to items in strList
function limitinput(evt, strList, bAllow) 
{ 
	var charCode = evt.keyCode; 
	if (charCode==0) 
	{ 
		charCode = evt.which; 
	} 
	var strChar = String.fromCharCode(charCode); 
	switch (charCode)
	{
		case 8:
		case 9:				
		return true;
		break;		
	}
		
	if (bAllow==true) 
	{	
		
 		return strList.indexOf(strChar)!=-1;
		
	} 
	else 
	{ 
	 	return strList.indexOf(strChar)==-1;		
	} 
} 

