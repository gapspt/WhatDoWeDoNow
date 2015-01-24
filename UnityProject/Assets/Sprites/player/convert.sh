for i in `ls *.gif`; do
 convert $i -resize 80x100 $i 
done
