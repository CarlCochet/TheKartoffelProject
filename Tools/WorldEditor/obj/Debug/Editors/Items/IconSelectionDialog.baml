   M S B A M L   `   `   `  ����F  BWorldEditor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:WorldEditor.HelpersWorldEditor.Helpers  5   	   3    -WorldEditor.Editors.Items.IconSelectionDialog   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   1vwp!clr-namespace:WorldEditor.Helpers     5   	     � Title$  Select Item Icon��5   	   $	��400��6"   $	��450��6/    � DataContext 5   	   ��  �� RelativeSource �� *Self+.��5      ]� .+�5	   
   A� $��Bottom=�6    A� Rows$ 1��6.   .+�5
      �� -   5        $��OK��5
      $��5q�6"    �� 	IsDefault . 6>   �� 5      -   5        $��Cancel��5       �� IsCancel . $��5q�66   6A   5      �� 5   
   $	��Top=�6   $��Disabled=�6'   $ �� IsSynchronizedWithCurrentItem$
 TrueY�6]   ��6�   �� *IconsSource+ �� SelectedValue 6�   �� *SelectedIcon+��5      �� 5       �� 
TargetType" ��6   .�5      �� 	 �� Property	 �@�OverridesDefaultStyle
 �� Value
 . 6O   �� 5      	 � ��$

 Left=�6K   �� 5      	 � ��$
 Center=�6I   �� 5      	 � ��
 5      �� 5       �� 
TargetType" ��6.   .R�5   "   �� $��Bd��6)   #��z��63   #��z��6e   #��z��6�    �� Padding# z��6�    J� SnapsToDevicePixels . ._�5   &   �� #��z��67   #��z��6�   # z 6�   5   #    �� Triggers 5   "   P� 5   &    P� Property �@��
IsSelected P� Value . . �5   *   ��  �� 
TargetName$ Bd��61   #
 � ��6A   	 � ��6�   �� 5   *   #
 � ��61   	 � ��6y   5   '   ]� 5   &    ]� 
Conditions 5   *   �� 5    .    �� Property �@��
IsSelected �� Value . 6Z   �� 5!   .    �@��IsSelectionActive .  6^   ./�5#   *   �� $ Bd��61   #
 � ��6A   	 � ��6�   �� 5$   *   #
 � ��61   	 � ��6w   5%   '   P� 5&   &    � {� .  . �5'   *   �� #
 � ��61   	 � ��6t   5(   '   5*      5,      5-      ��5/      �� 50       �� DataType$ 
icons:Icon��6   .R�51      ��  �� Orientation$ Vertical=�6!   .+�52      �� $��Center=�6    ��6=   �� *Image+�� 53      $��Center=�6$   ��6A   �� *Id+54      55      ��57      �� 58      .R�59      /   )WorldEditor.Helpers.VirtualizingWrapPanel     Orientation$ 
Horizontal=�60   $��Stretch=�6I   $	��Top=�6g   5:      5<      5=      5>      