   M S B A M L   `   `   `  ����  WorldEditor    WorldEditor.LoadingWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5          � WindowStyle$
  None=�5   	    � WindowStartupLocation$ CenterScreen=�6    � 
ResizeMode$ NoResize=�6A    � Topmost .  � Title$ LoadingWindow��5      $	��114��6.   $	��451��6;    � ShowInTaskbar .  ���	#FF2E7DA2 � DataContext 5   	   ��  �� RelativeSource �� *Self+.��5	      �� ���	#FFBCD2D3$��2q�6%   ._�5
   
   �� .+�5      �� $��45(�6   $��Center=�6"   ���	#FFFFFFFF$��WorldEditor��6V   $��0q�6l    J� Effect 5      3  -System.Windows.Media.Effects.DropShadowEffect  5      	   Color$	 White��6'   
   ShadowDepth$
 0Q�65      
BlurRadius$ 20Q�6E   5      �� 5      $��Center=�6    �� TextAlignment$ Center=�65   $��20(�6V   ���	#FF475366��6�   �� *
StatusText+ �� FallbackValue$ Loading ...��5      5      5      