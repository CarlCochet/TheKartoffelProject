   M S B A M L   `   `   `  ����  WorldEditor-    'WorldEditor.Editors.Files.D2O.D2OEditor   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   X TXceed.Wpf.Toolkit, Version=1.9.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f45xceed)http://schemas.xceed.com/wpf/xaml/toolkit  5   	   -   5          � Title$  	D2OEditor��5   	   $	��540��6   $	��700��6(   .��5      �  � RowDefinitions 5	   
   �� 5
      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   A�  A� Rows$ 1��6   $	��Top=�6   $
��Left=�64   $��0��6O   .+�5      �� $��3q�6   ��6    �� *
AddCommand+��6?   �� *SelectedItem+ �� ElementName$ NewItemTypeComboBox��.��5      ��  �� Orientation$ 
Horizontal=�6   .+�5      �� $��0,0,3,0q�6   $��16��6-   $8��2/WorldEditor;component/Images/action_add_16xlg.png��69   $
��Fill=�6u   $��16��6�   �� 5      $
��Wrap=�6     �� TextAlignment$ Center=�64   �� 5      $	�Add��6   5      5      5      �� 5      -   5        $��NewItemTypeComboBox��5      $��3q�62    �� DisplayMemberPath$
 Name��6=    �� SelectedIndex$ 0��6V   ��6h   �� *NewObjectTypes+�� 5      $��3q�6   ��6    �� *RemoveCommand+��6B   �� *SelectedItems+$ ObjectsGrid��.��5      �� $ 
Horizontal=�6   .+�5      �� $��0,0,3,0q�6   $��16��6-   $;��5/WorldEditor;component/Images/action_cancel_16xlg.png��69   $
��Fill=�6x   $��16��6�   �� 5      $
��Wrap=�6    $ Center=�64   �� 5      $�Remove��6   5      5      5      �� 5       $��3q�6   ��6    �� *ConvertCommand+.��5!      �� $��Convert to .xml��6   $
��Wrap=�63   $ Center=�6G   5"      �� 5#      $��3q�6   ��6    �� *SaveCommand+.��5$      �� $
��Save��6   $
��Wrap=�6(   $ Center=�6<   5%      �� 5&      $��3q�6   ��6    �� *SaveAsCommand+.��5'      �� $��Save as ...��6   $
��Wrap=�6/   $ Center=�6C   5(      5)      � 5*   
   $��1��6    � ColumnDefinitions 5+      �� 5,      $
��2.5* �6#   �� 5-      $
��Auto �6#    5/      �� 50      .+�52      9� $��Right=�6   	 9� ExpandDirection$	 Right=�6.   $��2��6F   ��53      �� 54      $��Edition��6    
 J� RenderTransformOrigin$
 0.5,0.5&�6/    � LayoutTransform 55      V� 56      .~�57   "   ��  �� ScaleX$ 1Q�61    �� ScaleY$ 1Q�6<   �� 58   "    �� AngleX$ 0Q�60    �� AngleY$ 0Q�6;   �� 59   "    �� Angle$	 -90Q�62   T� 5:   "    T� X$ 0Q�65    T� Y$ 0Q�6;   5;      z�5=      V� 5>      .~�5?   "   �� $ 1Q�61   $ 1Q�6<   �� 5@   "   $ 0Q�60   $ 0Q�6;   �� 5A   "   $ 0Q�62   T� 5B   "   $ 0Q�65   $ 0Q�6;   5C      5E      .��5G      1  +Xceed.Wpf.Toolkit.PropertyGrid.PropertyGrid  -   5        $��ObjectEditor��5G   %   $	��250��69      SelectedObject 6E   �� *SelectedItem+$ ObjectsGrid��5H      &   System.Windows.Controls.DataGrid  5I      -   5        $��ObjectsGrid��5I         AutoGenerateColumns .     
IsReadOnly .    AlternatingRowBackground �	#FFDCEAF8   GridLinesVisibility$ 
Horizontal=�6}      HorizontalGridLinesBrush �	#6A004BC1��6�   �� *Rows+ J� InputBindings 5J      �� 5K      
 �� Key$ Delete��6!    �� Command 6.   �� *RemoveCommand+ �� CommandParameter 6P   �� *SelectedItems+$ ObjectsGrid���� 5L      $
 Back��6!    6,   �� *RemoveCommand+ 6N   �� *SelectedItems+$ ObjectsGrid��5N      5O      �� 5P   
   $��2��6   $��3q�6   ���	#FFD3E1F1 �� CornerRadius$ 4��6A   $��2q�6R   ._�5Q      ]� .+�5R      �� $��Find ...��6   $��3q�6,   $��Center=�67   �� 5S      $ 0��6   $��Right=�6-   $��3q�6D   $��80��6O    �� SelectedValue 6Z   �� *SearchProperty+ �� Mode$ TwoWay=���6�   �� *SearchProperties+�� 5T      $
��Next��6   $��80��6(   $��Right=�63   $��3q�6J   ��6U   �� *FindNextCommand+�� 5U      $��Find !��6   $��80��6*   $��Right=�65   $��3q�6L   ��6W   �� *FindCommand+�� 5V      -   5        $��3q�5V      ��6%   �� *
SearchText+$ TwoWay=�  �� UpdateSourceTrigger$  PropertyChanged=�6s   5W      5X      5Y      5Z      