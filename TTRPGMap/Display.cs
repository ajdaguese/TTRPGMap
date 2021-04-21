using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTRPGMap
{
    /**
     * Display is a work in progress, for now I'm am testing displaying with bitmaps
     */
    class Display
    {
        private Bitmap bmp;
        private int[] pixelArray;
        //the constructor, for now, creates a 1000x1000 pixel 2d array of pixel values to store in the bitmap and the bitmap itslef
        //for now the entire picture will be a red and black checkerboard pattern with each square being 10x10 pixels and the bitmap's
        //window is 500x500
        public Display()
        {
            const int width = 1000;
            const int height = 1000;
            //I use unchecked to cast the hex literal from a uint to an int for selecting colors with our pixelFormat
            const int red = unchecked((int)0xffff0000);
            const int black = unchecked((int)0xffffffff);
            //a boolean to indicate if we're printing a red or black square
            bool isRed = false;
            bool startRed = true;
            pixelArray = new int[width * height];
            for (int i = 0; i < width; i += width * 10)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (startRed == true)
                    {
                        isRed = true;
                    }
                    else
                    {
                        isRed = false;
                    }
                    for (int k = 0; k < width; k++)
                    {
                        if (isRed)
                        {
                            pixelArray[i + (width * j) + k] = red;
                        }
                        else
                        {
                            pixelArray[i + (width * j) + k] = black;
                        }
                        if (k % 10 == 0)
                        {
                            isRed = !isRed;
                        }
                    }
                }
                startRed = !startRed;
            }
            bmp = new Bitmap(500, 500, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
        }
        public void draw()
        {
            // Lock the bitmap's bits.  
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            System.Drawing.Imaging.BitmapData bmpData =
                bmp.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite,
                bmp.PixelFormat);

            IntPtr ptr = bmpData.Scan0;
            int bytes = Math.Abs(bmpData.Stride) * bmp.Height;

            //load part of image into bitmap
            System.Runtime.InteropServices.Marshal.Copy(pixelArray, 0, ptr, bytes);
        }

        private int[] ConvertFullPictoPartial(int picArrayWidth, int picArrayHeight, int bmpWidth, int bmpHeight, int widthOffset, int heightOffset)
        {
            int[] partialPic = new int[bmpWidth * bmpHeight];
            for(int i = 0; i < bmpHeight; i++)
            {
                for(int j = 0; j < bmpWidth; j++)
                {
                    partialPic[i * bmpWidth + j] = pixelArray[(heightOffset * picArrayWidth) + widthOffset + j];
                }
            }
        }
    }
}
