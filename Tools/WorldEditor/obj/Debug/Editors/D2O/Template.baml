   M S B A M L   `   `   `  ����F  BWorldEditor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullM+clr-namespace:WorldEditor.Editors.Files.D2OWorldEditor.Editors.Files.D2O  5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089X TXceed.Wpf.Toolkit, Version=1.9.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f4A   ;Xceed.Wpf.Toolkit.Core.Converters.ObjectTypeToNameConverter   ObjectTypeToNameConverter   CollectionControlButtonStyle  �� 
TargetType �� Triggers P� Property P� Value �� Property �� ValueC   =WorldEditor.Editors.Files.D2O.DoublePrimitiveCollectionEditor �� 
TargetType � ColumnDefinitions � RowDefinitions	 �� SelectedIndex
 �� RelativeSource �� SelectedItem J� InputBindings
 �� Key �� Command �� CommandParameter �� ElementName6  0Xceed.Wpf.Toolkit.PrimitiveTypeCollectionControl   ItemsSource �� AcceptsReturn �� Mode:   4WorldEditor.Editors.Files.D2O.DoubleCollectionEditor � 	Resources
 � Tag �� DisplayMemberPath �� 	Converter �� Path1  +Xceed.Wpf.Toolkit.PropertyGrid.PropertyGrid   SelectedObject�� 5      D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    :x,http://schemas.microsoft.com/winfx/2006/xaml     5      5xceed)http://schemas.xceed.com/wpf/xaml/toolkit  5      3D2O+clr-namespace:WorldEditor.Editors.Files.D2O  5      %�  &	        &	      '  2    7  7  7  7  7  7  7  7  '  s    7  7  7  7  7   7  7  7  7     5      6&   �� 5      "  ��61    5	   
   P� 5
       � {� .  . �5      ��  �@J�Opacity$	 0.6Q�66   5      .�5   
   ��  � ��$ Center=�6?   �� 5   
    � ��$ Center=�6=   �� 5   
    � ��$ 26��64   �� 5   
    � ��$ 26��63   5      �� 5      "   6A   .�5   
   ��  � �� 5      �� 5      "  6"   .R�5      �� ._�5      �  5      �� 5   "   $��* �63   �� 5   "   $
��1.5* �63   .+�5      �  5   "   �� 5   &   �� 5    &   $
��Auto �67    5"   "   �� 5#   &   .+�5%   "   �� $��ListSelection��6*   $	 0��6?   ��6Q   �� *Items+
 �� *TemplatedParent+ 6�   �� *SelectedSubList+
 �� *TemplatedParent+ 5&   &   �� 5'   *   $ Delete��65   $  ApplicationCommands.Delete��6B    6g   �� *SelectedItem+$ ListSelection���� 5(   *   $
 Back��65   $  ApplicationCommands.Delete��6@    6e   �� *SelectedItem+$ ListSelection��5*   #   �� 5+   "   $��0,2,2,0q�6-   $	��Top=�6>   $��1��6V   $��36��6f   .+�5,   &   �� $��0,14,0,0q�6-   $	��New��6?   8��  .��5-   *   �� $
��None=�60   $��16��6?   $��16��6K   $��1q�6V   $��/Images/add.png��6a   5.   '   �� 5/   &   $��0,2,0,14q�6-   $ ��ApplicationCommands.Delete��6?   8�� ��6�   �� *SelectedItem+$ ListSelection��.��50   *   �� $
��None=�60   $��16��6?   $��16��6K   $��1q�6V   $��/Images/delete.png��6a   51   '   �� 52   &   8�� $��ComponentCommands.MoveUp��6c   ��6�   �� *SelectedItem+$ ListSelection��.��53   *   F� ���	#FF404040F������[     � � ��j � z ��j � z  � � 	=  � � 	= ��j �    ��j 	6@   54   '   �� 55   &   $��0,1,0,0q�6-   8�� $ ��ComponentCommands.MoveDown��6t   ��6�   �� *SelectedItem+$ ListSelection��.��56   *   F� ���	#FF404040F���� 	=     � z     � z @KL � � @KL р�[  � �    @KL � 	= @KL 	6@   57   '   58   #   59      � 5:      $��1��6#    5;   "   �� 5<   &   �� 5=   &   $
��Auto �67    5?   "   �� 5@   &   .+�5B   "   �� $��SubListSelection��6*   $	 0��6B   ��6T   �� *SelectedSubList+
 �� *TemplatedParent+�� 5C   "   $��0,2,2,0q�6-   $	��Top=�6>   $��1��6V   $��36��6f   .+�5D   &   �� $��0,14,0,0q�6-   $	��New��6?   8�� .��5E   *   �� $
��None=�60   $��16��6?   $��16��6K   $��1q�6V   $��/Images/add.png��6a   5F   '   �� 5G   &   $��0,2,0,14q�6-   $ ��ApplicationCommands.Delete��6?   8�� ��6�   �� *SelectedItem+$ ListSelection��.��5H   *   �� $
��None=�60   $��16��6?   $��16��6K   $��1q�6V   $��/Images/delete.png��6a   5I   '   �� 5J   &   8�� $��ComponentCommands.MoveUp��6c   ��6�   �� *SelectedItem+$ ListSelection��.��5K   *   F� ���	#FF404040F������[     � � ��j � z ��j � z  � � 	=  � � 	= ��j �    ��j 	6@   5L   '   �� 5M   &   $��0,1,0,0q�6-   8�� $ ��ComponentCommands.MoveDown��6t   ��6�   �� *SelectedItem+$ ListSelection��.��5N   *   F� ���	#FF404040F���� 	=     � z     � z @KL � � @KL р�[  � �    @KL � 	= @KL 	6@   5O   '   5P   #   5Q        5R      $��2��6C    6S   �� *SelectedItem+��5S   "   �� 5T   &   .R�5U   *   ��  . ��6G   �� *Text+
 �� *TemplatedParent+$ TwoWay=�5V   '   5X      5Y      5Z      5[      5]      5^      �� 5_      "   68   .�5`   
   ��  � �� 5a      �� 5b      "  6"   .R�5c      �� ._�5d      �  5e      �� 5f   "   $��* �63   �� 5g   "   $��* �63   �� 5h   "   $
��1.5* �63   .+�5j      � $��0��6#    5k   "   �� 5l   "   �� 5m   &   �� 5n   &   $
��Auto �67    5p   "   �� 5q   &   .+�5s   "   �� $��ListSelection��6*   $	 0��6?   ��6Q   �� *Items+
 �� *TemplatedParent+ 6�   �� *SelectedSubListSource+
 �� *TemplatedParent+ 5t   &   �� 5u   *   $ Delete��65   $  ApplicationCommands.Delete��6B    6g   �� *SelectedItem+$ ListSelection���� 5v   *   $
 Back��65   $  ApplicationCommands.Delete��6@    6e   �� *SelectedItem+$ ListSelection��5x   #   �� 5y   "   $��0,2,2,0q�6-   $	��Top=�6>   $��1��6V   $��36��6f   .+�5z   &   �� $��0,14,0,0q�6-   $	��New��6?   8��  ��6�   �� *SubListType+
 �� *TemplatedParent+ 6�   �� *Items+
 �� *TemplatedParent+.��5{   *   �� $
��None=�60   $��16��6?   $��16��6K   $��1q�6V   $��/Images/add.png��6a   5|   '   �� 5}   &   $��0,2,0,14q�6-   $ ��ApplicationCommands.Delete��6?   8�� ��6�   �� *SelectedItem+$ ListSelection�� 6�   �� *Items+
 �� *TemplatedParent+.��5~   *   �� $
��None=�60   $��16��6?   $��16��6K   $��1q�6V   $��/Images/delete.png��6a   5   '   �� 5�   &   8�� $��ComponentCommands.MoveUp��6c   ��6�   �� *SelectedItem+$ ListSelection�� 6�   �� *Items+
 �� *TemplatedParent+.��5�   *   F� ���	#FF404040F������[     � � ��j � z ��j � z  � � 	=  � � 	= ��j �    ��j 	6@   5�   '   �� 5�   &   $��0,1,0,0q�6-   8�� $ ��ComponentCommands.MoveDown��6t   ��6�   �� *SelectedItem+$ ListSelection�� 6�   �� *Items+
 �� *TemplatedParent+.��5�   *   F� ���	#FF404040F���� 	=     � z     � z @KL � � @KL р�[  � �    @KL � 	= @KL 	6@   5�   '   5�   #   5�      � 5�      $��1��6#    5�   "   �� 5�   &   �� 5�   &   $
��Auto �67    5�   "   �� 5�   &   $
��Auto �64   �� 5�   &   .+�5�   "   �� $��NewItemTypes��6+   $��0��6?   $��0,0,0,3q�6L   $��Stretch=�6]   $
 Name��6{   $	 0��6�   ��6�   �� *NewItemTypes+
 �� *TemplatedParent+�� 5�   "   $��SubListSelection��6*   $��1��6B   $	 0��6O   ��6a   �� *SelectedSubList+
 �� *TemplatedParent+ 6�   �� *SelectedItem+
 �� *TemplatedParent+��5�   &   �� 5�   *   .R�5�   .   �� ��68   �� 8  5�   +   5�   #   �� 5�   "   $��1��6-   $��0,2,2,0q�6:   $	��Top=�6K   $��1��6c   $��36��6s   .+�5�   &   �� $��0,14,0,0q�6-   $	��New��6?   8�� ��6�   �� $ NewItemTypes��$ SelectedItem� 6�   �� *SelectedSubList+
 �� *TemplatedParent+.��5�   *   �� $
��None=�60   $��16��6?   $��16��6K   $��1q�6V   $��/Images/add.png��6a   5�   '   �� 5�   &   $��0,2,0,14q�6-   $ ��ApplicationCommands.Delete��6?   8�� ��6�   �� *SelectedItem+$ SubListSelection�� 6�   �� *SelectedSubList+
 �� *TemplatedParent+.��5�   *   �� $
��None=�60   $��16��6?   $��16��6K   $��1q�6V   $��/Images/delete.png��6a   5�   '   �� 5�   &   8�� $��ComponentCommands.MoveUp��6c   ��6�   �� *SelectedItem+$ SubListSelection�� 6�   �� *SelectedSubList+
 �� *TemplatedParent+.��5�   *   F� ���	#FF404040F������[     � � ��j � z ��j � z  � � 	=  � � 	= ��j �    ��j 	6@   5�   '   �� 5�   &   $��0,1,0,0q�6-   8�� $ ��ComponentCommands.MoveDown��6t   ��6�   �� *SelectedItem+$ SubListSelection�� 6�   �� *SelectedSubList+
 �� *TemplatedParent+.��5�   *   F� ���	#FF404040F���� 	=     � z     � z @KL � � @KL р�[  � �    @KL � 	= @KL 	6@   5�   '   5�   #   5�        5�      $��2��61   $��0,5,0,0q�6A   $��Stretch=�6R   $��Stretch=�6p    6�   �� *SelectedItem+
 �� *TemplatedParent+5�      5�      5�      5�      5�      5�      