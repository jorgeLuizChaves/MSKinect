   M S B A M L   `   `   `  ����G  CImageByEvent, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null=#clr-namespace:ImageByEvent.AuxiliarImageByEvent.Auxiliar  5   	       ImageByEvent.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   h dMicrosoft.Kinect.Toolkit.Controls, Version=1.8.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35_ [Microsoft.Kinect.Toolkit, Version=1.8.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e352k(http://schemas.microsoft.com/kinect/2013   5   	   .my#clr-namespace:ImageByEvent.Auxiliar   5   	     � Title$  
MainWindow��5   	   $	��350��6   $
��1000��6)    � WindowState$ 	Maximized=�66   .��5      � $��
0,0,-558,0q�6    � ColumnDefinitions 5   
   �� 5	      $	��500 �6   �� 5
      $	��400 �6   �� 5      $	��100 �6     � RowDefinitions 5   
   �� 5      $��50 �6   �� 5      $��* �6   �� 5      $	��180 �6   .+�5   
   �� -   5        $��canvasKinect��5      $��1��6%   $��2��62   +   %ImageByEvent.Auxiliar.CanvasInteracao  5   
   -   5          Name$ canvasDesenho��5      $��1��64   $��2��6A   �� 5   
   -   5        $��slider��5      $��2��6   $��1��6/    �� Orientation$ Vertical=�6<   $��Center=�5       � Maximum$ 27Q�6.    � Minimum$	 -27Q�6;   $
��39,0q�6n   ��5      ��  �� ElementName$ imageKinect��	 �� Path$	 ActualHeight�5   n   �� 5   
   -   5        $��	eixoValor��5      $��2��6!   $��2��61   $��0��6>   $��35(�6J   $��Center=�6X   $��Center=�6u   $��35,62q�6�   4  .Microsoft.Kinect.Toolkit.KinectSensorChooserUI  5   
   -   5        $��kinectChooserUI��5   "   $��2��69   4  .Microsoft.Kinect.Toolkit.Controls.KinectRegion  5   
   -   5        $��KinectRegion��5      $��2��6-   $��3��6:   .��5      ]� .+�5      8  2Microsoft.Kinect.Toolkit.Controls.KinectTileButton  -   5        $��Fechar��5   %   $	��100��66   $	��150��6C   
�������$��Right=�6b   6y   �� 5      
 �� Orientation$
 
Horizontal=�6   .+�5      .   (ImageByEvent.Auxiliar.KinectToggleButton  -   5          Name$ chkEscalaCinza��5   ,   $��Monocromático��6D   $	��100��6\   $	��150��6j   $��20(�6v   
�������6�     5       -	   5        $ chkEsqueleto��5    ,   $��	Esqueleto��6B   $	��100��6V   $	��150��6c   $��20(�6o   
�������6}     5!      -
   5        $ btnDesenhar��5!   ,   .   (ImageByEvent.Auxiliar.KinectToggleButton   Click$ btnDesenharClick��6�   $��Desenhar��6A   $	��100��6T   $	��150��6a   $��20(�6m   
�������6�     5"      -   5        $��Desligar��5"   )   $	��100��6<   $	��150��6I   
�������6h   5#      5$      5%      5&      5'      