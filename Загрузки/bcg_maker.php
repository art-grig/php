    <?php
$height = 550;
$width = 550;
$im = imagecreatetruecolor($width, $height);
$white = imagecolorallocate ($im, 255, 255, 255);
imagefill($im, 0, 0, $white);
$black = imagecolorallocate ($im, 0, 0, 0);
$red = imagecolorallocate($im, 233, 14, 91);
//imagestring($im, 5, 5, 5,  'Problem childs', $black);
//imagestring($im, 5, $width-50, 5,  'Stars', $black);
//imagestring($im, 5, 5, $height/2+5,  'Dogs', $black);
//imagestring($im, 5, $width-50,  $height/2+5,  'Cows', $black);

imageline($im,$width/2,0,$width/2+5,5,$black);			//Y arrows creating
imageline($im,$width/2,0,$width/2-5,5,$black);

imageline($im,$width,$height/2,$width-5,$height/2+5,$black);			// X arrows creating
imageline($im,$width,$height/2,$width-5,$height/2-5,$black);

imageline($im,$width/2,0,$width/2,$height,$black);			//coordinate plane creating
imageline($im,0,$height/2,$width,$height/2,$black);
//imageellipse($im, $width/4, $height/4, 40, 40, $red);
$i=10;
for (;$i<$height;) {
	imageline($im,$width/2-3,0+$i,$width/2+3,0+$i,$black);
	imageline($im,0+$i,$height/2-3,0+$i,$height/2+3,$black);
	$i = $i+10;
}

//imagestring($im, 10, 5, $height/2+5,  $i, $black);
$handle = fopen("bcg_output_2.txt", "r");
if ($handle) {
    $MINT = fgets($handle);
    $SRT = fgets($handle);
    $MAXT = fgets($handle);
    $shag = intval($height/intval(($MAXT-$MINT)/0.3));
    $size = fgets($handle);
    for ($count=0;$count<$size;++$count) {
    	$name = fgets($handle);
    	$export = fgets($handle);
    	$export = intval($export/80);
    	$relative_share = fgets($handle);
    	$growth_rate = fgets($handle);
    	$buf = $growth_rate;
    	$growth_rate = intval((($MAXT-$growth_rate)/0.3)+1)*$shag;
    	imageellipse($im, $relative_share, $growth_rate, $export, $export, $red);
    	imagestring($im, 3, $relative_share, $growth_rate, intval($name), $black);
    	if ($buf!==$MINT and $buf!==$MAXT)
    	imagestring($im, 1, $width/2+5, $growth_rate,  $buf, $black);
    }
} else {
    // error opening the file.
} 
//imagestring($im, 2, $width/2+5, $height/2+5,  $SRT, $black);
imagestring($im, 2, $width/2+5, 0+5,  $MAXT, $black);
imagestring($im, 2, $width/2+5, $height-15,  $MINT, $black);
$MAXT = intval($MAXT*10);
$SRT = intval($SRT*10);
$MINT = intval($MINT*10);
fclose($handle);

header("Content-type: image/png");
imagepng($im);
?>