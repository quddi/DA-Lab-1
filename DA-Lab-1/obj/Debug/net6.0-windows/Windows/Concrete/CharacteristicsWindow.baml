   M S B A M L   `   `   `  ����C  ?DA-Lab-1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:DA_Lab_1DA_Lab_1  5   	   $    DA_Lab_1.CharacteristicsWindow   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QScottPlot.WPF, Version=4.1.67.0, Culture=neutral, PublicKeyToken=e53b06131e34a3aae aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   (localclr-namespace:DA_Lab_1     5   	     � Title$  CharacteristicsWindow��5   	   $	��324��6'   $
��1193��64   .��5	      � $	��289��6   $	��Top=�6   $��Center=�60   $
��1180��6M   .+�5
   
   �� $��	0,1,386,0q�6   $5��//Windows/Concrete/CharacteristicsBackground.png��6#   $	��212��6\   $	��Top=�6i   �� 5   
   -   5         ��Name$ MeanGradeText��5      $
��Left=�6)   $��
305,48,0,0q�6D    �� TextWrapping$
 Wrap=�6X   $��TextBox��6l   $	��Top=�6{   $��75��6�   $��23��6�    �� TextAlignment$ Center=�6�   �� 5   
   -   5        $ MedianGradeText��5      $
��Left=�6+   $��
305,71,0,0q�6F   $
 Wrap=�6Z   $��TextBox��6n   $	��Top=�6}   $��75��6�   $��23��6�   $ Center=�6�   �� 5   
   -   5        $  StandardDeviationGradeText��5      $
��Left=�66   $��
305,94,0,0q�6Q   $
 Wrap=�6e   $��TextBox��6y   $	��Top=�6�   $��75��6�   $��22��6�   $ Center=�6�   �� 5   
   -   5        $" SkewnessCoefficientGradeText��5      $
��Left=�68   $��305,116,0,0q�6S   $
 Wrap=�6h   $��TextBox��6|   $	��Top=�6�   $��75��6�   $��23��6�   $ Center=�6�   �� 5   
   -   5        $" KurtosisCoefficientGradeText��5      $
��Left=�68   $��305,139,0,0q�6S   $
 Wrap=�6h   $��TextBox��6|   $	��Top=�6�   $��75��6�   $��22��6�   $ Center=�6�   �� 5   
   -   5        $ MinGradeText��5      $
��Left=�6(   $��305,161,0,0q�6C   $
 Wrap=�6X   $��TextBox��6l   $	��Top=�6{   $��75��6�   $��23��6�   $ Center=�6�   �� 5   
   -   5        $ MaxGradeText��5      $
��Left=�6(   $��305,184,0,0q�6C   $
 Wrap=�6X   $��TextBox��6l   $	��Top=�6{   $��75��6�   $��22��6�   $ Center=�6�   �� 5   
   -   5        $ MeanMSDEText��5      $
��Left=�6(   $��
380,48,0,0q�6C   $
 Wrap=�6W   $��TextBox��6k   $	��Top=�6z   $	��202��6�   $��23��6�   $ Center=�6�   �� 5   
   -	   5        $ MedianMSDEText��5      $
��Left=�6*   $��
380,71,0,0q�6E   $
 Wrap=�6Y   $��-��6m   $	��Top=�6v   $	��202��6�   $��23��6�   $ Center=�6�   �� 5   
   -
   5        $ StandardDeviationMSDEText��5      $
��Left=�65   $��
380,94,0,0q�6P   $
 Wrap=�6d   $��TextBox��6x   $	��Top=�6�   $	��202��6�   $��22��6�   $ Center=�6�   �� 5   
   -   5        $! SkewnessCoefficientMSDEText��5      $
��Left=�67   $��380,116,0,0q�6R   $
 Wrap=�6g   $��TextBox��6{   $	��Top=�6�   $	��202��6�   $��23��6�   $ Center=�6�   �� 5   
   -   5        $! KurtosisCoefficientMSDEText��5      $
��Left=�67   $��380,139,0,0q�6R   $
 Wrap=�6g   $��TextBox��6{   $	��Top=�6�   $	��202��6�   $��22��6�   $ Center=�6�   �� 5   
   -   5        $ MinMSDEText��5      $
��Left=�6'   $��380,161,0,0q�6B   $
 Wrap=�6W   $��-��6k   $	��Top=�6t   $	��202��6�   $��23��6�   $ Center=�6�   �� 5   
   -   5        $ MaxMSDEText��5      $
��Left=�6'   $��380,184,0,0q�6B   $
 Wrap=�6W   $��-��6k   $	��Top=�6t   $	��202��6�   $��22��6�   $ Center=�6�   �� 5   
   -   5        $ MeanTrustIntervalText��5      $
��Left=�61   $��
582,48,0,0q�6L   $
 Wrap=�6`   $��TextBox��6t   $	��Top=�6�   $	��208��6�   $��23��6�   $ Center=�6�   �� 5   
   -   5        $ MedianTrustIntervalText��5      $
��Left=�63   $��
582,71,0,0q�6N   $
 Wrap=�6b   $��TextBox��6v   $	��Top=�6�   $	��208��6�   $��23��6�   $ Center=�6�   �� 5   
   -   5        $( "StandardDeviationTrustIntervalText��5      $
��Left=�6>   $��
582,94,0,0q�6Y   $
 Wrap=�6m   $��TextBox��6�   $	��Top=�6�   $	��208��6�   $��22��6�   $ Center=�6�   �� 5   
   -   5        $* $SkewnessCoefficientTrustIntervalText��5      $
��Left=�6@   $��582,116,0,0q�6[   $
 Wrap=�6p   $��TextBox��6�   $	��Top=�6�   $	��208��6�   $��23��6�   $ Center=�6�   �� 5   
   -   5        $* $KurtosisCoefficientTrustIntervalText��5      $
��Left=�6@   $��582,139,0,0q�6[   $
 Wrap=�6p   $��TextBox��6�   $	��Top=�6�   $	��208��6�   $��22��6�   $ Center=�6�   �� 5   
   -   5        $ MinTrustIntervalText��5      $
��Left=�60   $��582,161,0,0q�6K   $
 Wrap=�6`   $��-��6t   $	��Top=�6}   $	��208��6�   $��23��6�   $ Center=�6�   �� 5   
   -   5        $ MaxTrustIntervalText��5      $
��Left=�60   $��582,184,0,0q�6K   $
 Wrap=�6`   $��-��6t   $	��Top=�6}   $	��208��6�   $��22��6�   $ Center=�6�     ScottPlot.WpfPlot  5    
   -   5          Name$ ProbabilityPaperChart��5       $��794,5,10,83q�61   �� 5!   
   -   5         ��Name$ IdentifyBySkewnessButton��5!      $o��iІдентифікувати розподіл на основі коефіцієнту асиметрії��63   $
��Left=�6u   $��
5,215,0,10q�6�   $	��375��6�   6�   �� 5"   
   -   5        $ IdentifyByKurtosisButton��5"      $k��eІдентифікувати розподіл на основі коефіцієнту ексцесу��63   $
��Left=�6s   $��385,215,0,10q�6�   $	��405��6�   6�   �� 5#   
   -   5        $! BuildProbabilityPaperButton��5#      $k��eІдентифікувати розподіл на основі коефіцієнту ексцесу��66   $
��Left=�6v   $��795,215,0,10q�6�   $	��375��6�   6�   5$      5%      