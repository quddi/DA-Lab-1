   M S B A M L   `   `   `  ����C  ?DA-Lab-1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:DA_Lab_1DA_Lab_1  5   	       DA_Lab_1.MainWindow   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QScottPlot.WPF, Version=4.1.67.0, Culture=neutral, PublicKeyToken=e53b06131e34a3aae aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   (localclr-namespace:DA_Lab_1     5   	     � Title$  
MainWindow��5   	   $	��675��6   $
��1284��6)   .��5	      � .+�5
   
   &   System.Windows.Controls.DataGrid  -   5          Name$ GroupedDataGrid��5
      $��	10,10,0,0q�6W      CanUserSortColumns$ False��6j      CanUserResizeColumns$ False��6�      CanUserReorderColumns$ False��6�      AutoGenerateColumns$ False��6�      CanUserAddRows$ False��6�      CanUserDeleteRows$ False��6�   $	��387��6  $	��Top=�6  $
��Left=�62  $	��475��6M     Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn	   Header$		 №��6%   
   CanUserResize$
 False��60      Width5  /System.Windows.Controls.DataGridLengthConverter$ 30 6F     5      $	 Значення��6%   $
 False��67   $ 70 6M     5      $	 Частота��6%   $
 False��66   $ 60 6L     5      $%	 Відносна частота��6%   $
 False��6?   $	 110 6U     5      $$	 Значення емп. ф-ї��6%   $
 False��6@      FontSize$ 12Q�6V   $	 120 6d   4  .System.Windows.Controls.DataGridCheckBoxColumn  5      $	 Аномальне��6)   $
 False��6<   $ * 6R   5      �� 5   
   -   5         ��Name$ UploadFileButton��5      $%��Завантажити файл��6+   $��968,10,10,0q�6d   $��39��6y   $	��Top=�6�   #��� ��6�   �� 5   
   -   5        $ ClassifyDataButton��5      $)��#   Поділити на класи��6-   $	��Top=�6L   $��39��6d   $��968,54,10,0q�6p   $
��Left=�6�   #��� ��6�   �� 5   
   -   5         ��Name$ ClassesAmountTextBox��5      $��1088,59,18,0q�60    �� TextWrapping$
 Wrap=�6F   $'��!*кількість класів*��6Z   $��Center=�6t   $��Center=�6�   $��10(�6�   $��Italic$�6�   $��30��6�   $	��Top=�6�     5   
   -   5        $ ClassedDataGrid��5      $��
490,10,0,0q�6W   $ False��6k   $ False��6�   $ False��6�   $ False��6�   $ False��6�   $ False��6�   $	��387��6  $	��Top=�6  $
��Left=�63  $	��473��6N   5        5      $		 №��6%   $
 False��60   $ 30 6F     5      $	 Межі класу��6%   $
 False��69   $	 120 6O     5      $	 Частота��6%   $
 False��66   $ 60 6L     5      $%	 Відносна частота��6%   $
 False��6?   $	 110 6U     5      $-	 'Значення емп. функції��6%   $
 False��6D   $ 12Q�6Z   $ * 6h   5        ScottPlot.WpfPlot  5    
   -   5          Name$ BarChart��5       $��
10,402,0,0q�6$   $	��247��68   $	��Top=�6E   $
��Left=�6]   $	��391��6x   �� 5!   
   -   5        $" ComputeCharacteristicsButton��5!      $7��1Обрахувати характеристики��67   $��968,98,10,0q�6�   $��39��6�   $	��Top=�6�   #��� ��6�   �� 5"   
   -   5        $0 *BuildKernelDensityEstimationFunctionButton��5"      $>��8Побудувати КДЕ                             ��6E   $��968,142,10,0q�6{   $��45��6�   $	��Top=�6�   #��� ��6�   $��	-81,1,1,1q�64    5#   
   -	   5        $  CumulativeProbabilityChart��5#      $��406,402,0,0q�66   $
��Left=�6K   $	��429��6f   $	��247��6r   $	��Top=�6   �� 5$   
   -
   5        $/ )BuildComulativeProbabilityHistogramButton��5$      $.��(Побудувати графік ЕФР��6D   $��968,192,10,0q�6d   $	��Top=�6z   $��40��6�   #��� ��6�   �� 5%   
   -   5        $ BandWidthTextBox��5%      $��1088,146,18,0q�6,   $
 Wrap=�6C   $��*ширина вікна*��6W   $��Center=�6m   $��Center=�6�   $��Italic$�6�   $��36��6�   $	��Top=�6�   �� 5&   
   $��968,237,10,0q�6   ���	#FFE6E6E6
���   �$��87��6J   $	��Top=�6V   �� 5'   
   -   5         ��Name$ KParameterSlider��5'      $��1015,296,50,0q�6+    � Maximum$ 3Q�6B    � Value$	 1.5Q�6N    � Minimum$	 1.5Q�6Z    �� TickFrequency$	 0.1Q�6h    J� RenderTransformOrigin$ 0.502,0.485&�6|   $��19��6�   $	��Top=�6�   #��� ��6�   �� 5(   
   $'��!Ввести значення K =��6   $��978,242,0,0q�6.   $	��Top=�6C   $
��Left=�6[   $	��120��6v   �� 5)   
   -   5        $ KParameterTextBox��5)      $��1103,242,18,0q�6-   $
 Wrap=�6D   $��K=1,5��6X   $	��Top=�6e   $��26��6}   $��0,4,0,0q�6�    �� TextAlignment$ Center=�6�   �� 5*   
   $	��1.5��6   $
��Left=�6   $��983,293,0,0q�69   $	��Top=�6N   $
��Bold"�6f   �� 5+   
   $��3��6   $��
0,293,29,0q�6   $
��Bold"�60   $��26��6B   $	��Top=�6N   $��Right=�6f   $��17��6�   �� 5,   
   -   5        $# DeleteOutlieGroupedDataButton��5,      $:��4Видалити аномальні значення��68   $��968,364,10,0q�6^   #��� ��6�   $��32��6�   $	��Top=�6�   �� 5-   
   $=��7для пошуку аномальних значень��6   $��1022,268,64,0q�68   $	��Top=�6O     5.   
   -   5        $ AnomaliesPointsChart��5.      $��845,402,0,0q�60   $	��429��6E   $	��247��6Q   $	��Top=�6^   $��Stretch=�6v   $
��Left=�6�   $
��Auto=�6�   �� 5/   
   -   5        $ FindAnomaliesButton��5/      $F��@Ідентифікувати аномальні значення��6.   $��968,329,10,0q�6Z   #��� ��6p   $��30��6�   $	��Top=�6�   6�   50      51      