   M S B A M L   `   `   `  ����  WorldEditor-    'WorldEditor.Editors.Files.D2I.D2IEditor   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   $	��600��5   	   $	��700��6   $	��700��6"     � Title  61   �� *Title+.��5      �  � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5	      $��* �6   �� 5
      $
��Auto �6   �� 5      -   5         �Name$ ExpanderRow��5      $
��Auto �6/   �� 5      $
��Auto �6   .+�5   
   A�  A� Rows$ 1��6   $	��Top=�6   $
��Left=�64   $��0��6O   .+�5      �� $��3q�6   ��6    �� *AddRowCommand+.��5      ��  �� Orientation$ 
Horizontal=�6   .+�5      �� $��0,0,3,0q�6   $��16��6-   $8��2/WorldEditor;component/Images/action_add_16xlg.png��69   $
��Fill=�6u   $��16��6�   �� 5      $
��Wrap=�6     �� TextAlignment$ Center=�64   �� 5      $�Add Text��6   5      5      5      �� 5      $��3q�6   ��6    �� *AddUIRowCommand+.��5      �� $ 
Horizontal=�6   .+�5      �� $��0,0,3,0q�6   $��16��6-   $8��2/WorldEditor;component/Images/action_add_16xlg.png��69   $
��Fill=�6u   $��16��6�   �� 5      $ Center=�6    $��Add UI Text��6A   5      5      �� 5      $��3q�6   ��6    �� *RemoveRowCommand+��6E   �� *SelectedItems+ �� ElementName$ 	TextsGrid��.��5      �� $ 
Horizontal=�6   .+�5      �� $��0,0,3,0q�6   $��16��6-   $;��5/WorldEditor;component/Images/action_cancel_16xlg.png��69   $
��Fill=�6x   $��16��6�   �� 5       $
��Wrap=�6    $ Center=�64   �� 5!      $�Remove��6   5"      5#      5$      �� 5%      $��3q�6   ��6    �� *ConvertToTxtCommand+.��5&      �� $ 
Horizontal=�6   .+�5'      �� $��0,0,3,0q�6   $��16��6-   $9��3/WorldEditor;component/Images/nextrequest_10302.png��69   $
��Fill=�6v   $��16��6�   �� 5(      $
��Wrap=�6    $ Center=�64   $��Convert to .txt��6U   5)      5*      �� 5+      $��3q�6   ��6    �� *SaveCommand+.��5,      �� $ 
Horizontal=�6   .+�5-      �� $��0,0,3,0q�6   $��16��6-   $1��+/WorldEditor;component/Images/Save_6530.png��69   $
��Fill=�6n   $��16��6}   �� 5.      $
��Wrap=�6    $ Center=�64   $
��Save��6U   5/      50      �� 51      $��3q�6   ��6    �� *SaveAsCommand+.��52      �� $ 
Horizontal=�6   .+�53      �� $��0,0,3,0q�6   $��16��6-   $1��+/WorldEditor;component/Images/Save_6530.png��69   $
��Fill=�6n   $��16��6}   �� 54      $
��Wrap=�6    $ Center=�64   $��Save as ...��6U   55      56      57      9� 58   
   -   5        $��EditExpander��58      $��Bottom=�6'   $
��Edit��6?    9� 
IsExpanded .  9� ExpandDirection$
 Down=�6_   $��3��6v   ��59      �� 5:      	 �� 
TargetType"	 9�6   .�5;      �� 
 �� Property
 � y� �� Value$ Visible=�6=    �� Triggers 5<      �� 5=       �� Binding 6&   �� *SelectedIndex+$ 	TextsGrid�� �� Value -16_   .a�5>      �� 
 � y�$ 	Collapsed=�6E   5?      5A      .��5C      �  � IsSharedSizeScope . $��3q�61    5D      �� 5E      $
��Auto �6$   �� 5F      $��* �6$   .+�5H      ��  �� TextWrapping$
 Wrap=�6   $��3q�6.   $��1��69   ��6I   �� $ 	TextsGrid�� �� Path$ SelectedItem.Id� �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=��� 5I      $
 Wrap=�6   $��3q�6.   $��1��69   $��1��6I   " �� VerticalScrollBarVisibility$
 Auto=�6V    �� AcceptsReturn .  �� 
AcceptsTab . ��6�   �� $ 	TextsGrid��$ SelectedItem.Text�$ TwoWay=�$ PropertyChanged=�5J      5K      �� 5L   
   $��2��6   $��Stretch=�6$   $��5��6B   ���	#FFA5C0EF �� ResizeDirection$
 Rows=�6d    J� Opacity$	 0.5Q�6{   ��5M      �� 5N      "	 ��6   .�5O      �� 
 � {� . 6<    5P      �� 5Q       6&   �� *SelectedIndex+$ 	TextsGrid�� -16_   .a�5R      �� 
 � {� .  6D   5S      �� 5T       6&   �� *
IsExpanded+$ EditExpander��	 false6_   .a�5U      �� 
 � {� .  6D   5V      5X      5Z      &   System.Windows.Controls.DataGrid  5[   
   -   5        $��	TextsGrid��5[         AutoGenerateColumns .     GridLinesVisibility$
 None=�6@      HorizontalGridLinesBrush �	#82000000   AlternatingRowBackground �	#FFDCEAF8   RowDetailsVisibilityMode$ 	Collapsed=�6�   $��1��6�   ��6�   �� *Rows+ J� InputBindings 5\      �� 5]      
 �� Key$ Delete��6     �� Command  6*   �� *RemoveRowCommand+! �� CommandParameter! 6O   �� $ 	TextsGrid��$ SelectedItems��� 5^      $
 Back��6     6(   �� *RemoveRowCommand+! 6M   �� $ 	TextsGrid��$ SelectedItems�"   Columns" 5`      0  *System.Windows.Controls.DataGridTextColumn  5a      ,  &System.Windows.Controls.DataGridColumn#   Header$	# Key��6%   $   Width5  /System.Windows.Controls.DataGridLengthConverter$	$ 100 62   %   
IsReadOnly% . 1  +System.Windows.Controls.DataGridBoundColumn&   Binding& 6P   �� *Id+  5b      $# Value��6%   $$ * 64   % . & 6P   �� *Text+5d      �� 5e   
   $��4��6   $��3q�6   ���	#FFD3E1F1' �� CornerRadius$' 4��6A   $��2q�6R   ._�5f      ]� .+�5g      �� $��Find ...��6   $��3q�6,   $��Center=�67   �� 5h      ( �� SelectedIndex$( 0��6   $��Right=�6-   $��3q�6D   $��80��6O   ) �� SelectedValuePath$) Content��6Z   * �� SelectedValue* 6v   �� *
SearchType+$ TwoWay=�.@�5i      �� .��6#   Text6)   �� 5j      .��6#   Key6(   5k      �� 5l      $
��Next��6   $��80��6(   $��Right=�63   $��3q�6J   ��6U   �� *FindNextCommand+�� 5m      $��Find !��6   $��80��6*   $��Right=�65   $��3q�6L   ��6W   �� *FindCommand+�� 5n      -   5        $��3q�5n      ��6%   �� *
SearchText+$ TwoWay=�$ PropertyChanged=�6s   5o      5p      5q      5r      