   M S B A M L   `   `   `  ����  WorldEditor*    $WorldEditor.Editors.Langs.LangEditor   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  Lang Editor��5   	   $	��600��6   $	��700��6*   $	��700��66    � 	Resources 5         BooleanToVisibility��%   &	        �� 5   
   6'   5      .��5      �  � RowDefinitions 5	   
   �� 5
      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   �� 5      -   5         �Name$ ExpanderRow��5      $
��Auto �6/   �� 5      $
��Auto �6   �� 5      $
��Auto �6   .+�5   
   A�  A� Rows$ 1��6   $	��Top=�6   $
��Left=�64   $��0��6O   .+�5      �� $��3q�6   ��6    �� *AddRowCommand+.��5      ��  �� Orientation$ 
Horizontal=�6   .+�5      �� $��0,0,3,0q�6   $��16��6-   $8��2/WorldEditor;component/Images/action_add_16xLG.png��69   $
��Fill=�6u   $��16��6�   �� 5      $
��Wrap=�6     �� TextAlignment$ Center=�64   �� 5      $�Add Text��6   5      5      5      �� 5      $��3q�6   ��6    �� *AddUIRowCommand+.��5      �� $ 
Horizontal=�6   .+�5      �� $��0,0,3,0q�6   $��16��6-   $8��2/WorldEditor;component/Images/action_add_16xLG.png��69   $
��Fill=�6u   $��16��6�   �� 5      $ Center=�6    $��Add UI Text��6A   5      5      �� 5       $��3q�6   ��6    �� *RemoveRowCommand+��6E   �� *SelectedItems+ �� ElementName$ 	TextsGrid��.��5!      �� $ 
Horizontal=�6   .+�5"      �� $��0,0,3,0q�6   $��16��6-   $;��5/WorldEditor;component/Images/action_Cancel_16xLG.png��69   $
��Fill=�6x   $��16��6�   �� 5#      $
��Wrap=�6    $ Center=�64   �� 5$      $�Remove��6   5%      5&      5'      �� 5(      $��3q�6   ��6    �� *SaveCommand+.��5)      �� $ 
Horizontal=�6   .+�5*      �� $��0,0,3,0q�6   $��16��6-   $1��+/WorldEditor;component/Images/Save_6530.png��69   $
��Fill=�6n   $��16��6}   �� 5+      $
��Wrap=�6    $ Center=�64   $
��Save��6U   5,      5-      5.      9� 5/   
   -   5        $��EditExpander��5/      $��Bottom=�6'   $
��Edit��6?    9� 
IsExpanded . 	 9� ExpandDirection$
	 Down=�6_   $��3��6v   ��50      �� 51      
 �� 
TargetType"
 9�6   .�52      ��  �� Property � y� �� Value$ Visible=�6=    �� Triggers 53      �� 54       �� Binding 6&   �� *SelectedIndex+$ 	TextsGrid�� �� Value -16_   .a�55      ��  � y�$ 	Collapsed=�6E   56      58      .��5:      �  � IsSharedSizeScope . $��3q�61    5;      �� 5<      $
��Auto �6$   �� 5=      $��* �6$   .+�5?      ��  �� TextWrapping$
 Wrap=�6   $��3q�6.   ��69   �� $ 	TextsGrid�� �� Path$ SelectedItem.Id� �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=��� 5@      $��1��6   .@�5A      �� $��French��6   .��5B      �� $
 Wrap=�6"   $��3q�66   " �� VerticalScrollBarVisibility$
 Auto=�6A    �� AcceptsReturn .  �� 
AcceptsTab . ��6�   �� $ 	TextsGrid��$ SelectedItem.French�$ TwoWay=�$ PropertyChanged=�5C      �� 5D      $��English��6   .��5E      �� $
 Wrap=�6"   $��3q�66   $
 Auto=�6A    .  . ��6�   �� $ 	TextsGrid��$ SelectedItem.English�$ TwoWay=�$ PropertyChanged=�5F      �� 5G      $��Dutsh��6   .��5H      �� $
 Wrap=�6"   $��3q�66   $
 Auto=�6A    .  . ��6�   �� $ 	TextsGrid��$ SelectedItem.Dutsh�$ TwoWay=�$ PropertyChanged=�5I      �� 5J      $��	Portugese��6   .��5K      �� $
 Wrap=�6"   $��3q�66   $
 Auto=�6A    .  . ��6�   �� $ 	TextsGrid��$ SelectedItem.Portugese�$ TwoWay=�$ PropertyChanged=�5L      �� 5M      $��German��6   .��5N      �� $
 Wrap=�6"   $��3q�66   $
 Auto=�6A    .  . ��6�   �� $ 	TextsGrid��$ SelectedItem.German�$ TwoWay=�$ PropertyChanged=�5O      �� 5P      $��Russish��6   .��5Q      �� $
 Wrap=�6"   $��3q�66   $
 Auto=�6A    .  . ��6�   �� $ 	TextsGrid��$ SelectedItem.Russish�$ TwoWay=�$ PropertyChanged=�5R      �� 5S      $��Spanish��6   .��5T      �� $
 Wrap=�6"   $��3q�66   $
 Auto=�6A    .  . ��6�   �� $ 	TextsGrid��$ SelectedItem.Spanish�$ TwoWay=�$ PropertyChanged=�5U      �� 5V      $��Italian��6   .��5W      �� $
 Wrap=�6"   $��3q�66   $
 Auto=�6A    .  . ��6�   �� $ 	TextsGrid��$ SelectedItem.Italian�$ TwoWay=�$ PropertyChanged=�5X      5Y      5Z      5[      �� 5\   
   $��2��6   $��Stretch=�6$   $��5��6B   ���	#FFA5C0EF �� ResizeDirection$
 Rows=�6d    J� Opacity$	 0.5Q�6{   ��5]      �� 5^      "
 ��6   .�5_      ��  � {� . 6<    5`      �� 5a       6&   �� *SelectedIndex+$ 	TextsGrid�� -16_   .a�5b      ��  � {� .  6D   5c      �� 5d       6&   �� *
IsExpanded+$ EditExpander��	 false6_   .a�5e      ��  � {� .  6D   5f      5h      5j      &   System.Windows.Controls.DataGrid  5k   
   -   5        $��	TextsGrid��5k         AutoGenerateColumns .     GridLinesVisibility$
 None=�6@      HorizontalGridLinesBrush �	#82000000   AlternatingRowBackground �	#FFDCEAF8   RowDetailsVisibilityMode$ 	Collapsed=�6�   $��1��6�   ��6�   �� *Rows+ J� InputBindings 5l      �� 5m      
  �� Key$  Delete��6   ! �� Command! 6*   �� *RemoveRowCommand+" �� CommandParameter" 6O   �� $ 	TextsGrid��$ SelectedItems��� 5n      $
  Back��6   ! 6(   �� *RemoveRowCommand+" 6M   �� $ 	TextsGrid��$ SelectedItems�#   Columns# 5p      0  *System.Windows.Controls.DataGridTextColumn  5q      ,  &System.Windows.Controls.DataGridColumn$   Header$	$ Key��6%   %   Width5  /System.Windows.Controls.DataGridLengthConverter$	% 100 62   &   
IsReadOnly& . 1  +System.Windows.Controls.DataGridBoundColumn'   Binding' 6P   �� *Id+  5r      $$ French��6%   $% * 65   ' 6?   �� *French+(   
Visibility( 6Z   �� ) �� Source) %  System.Windows.Markup.Reference  *FrenchVisibleBox+$ 	IsChecked�* �� 	Converter#* [    5s      $$ English��6%   $% * 66   ' 6@   �� *	English+( 6\   �� )   *EnglishVisibleBox+$ 	IsChecked�#* [    5t      $$ German��6%   $% * 65   & . ' 6Q   �� *German+( 6l   �� )   *GermanVisibleBox+$ 	IsChecked�#* [    5u      $$ Dutsh��6%   $% * 64   & . ' 6P   �� *Dutsh+( 6j   �� )   *DutshVisibleBox+$ 	IsChecked�#* [    5v      $$ Italian��6%   $% * 66   & . ' 6R   �� *	Italian+( 6n   �� )   *ItalianVisibleBox+$ 	IsChecked�#* [    5w      $$ Spanish��6%   $% * 66   & . ' 6R   �� *	Spanish+( 6n   �� )   *SpanishVisibleBox+$ 	IsChecked�#* [    5x      $$ 	Portugese��6%   $% * 68   & . ' 6T   �� *	Portugese+( 6r   �� )   *PortugeseVisibleBox+$ 	IsChecked�#* [    5y      $$ Russish��6%   $% * 66   & . ' 6R   �� *	Russish+( 6n   �� )   *RussishVisibleBox+$ 	IsChecked�#* [    5z      $$ Japanish��6%   $% * 67   & . ' 6S   �� *
Japanish+( 6p   �� )   *JapanishVisibleBox+$ 	IsChecked�#* [  5|      �� 5}   
   $��4��6   $��3q�6"   $ 
Horizontal=�6-   .+�5~      �� -   5        $��FrenchVisibleBox��5~      $��3q�6/   $��French��6:   + d� 	IsChecked$
+ TrueY�6K   �� 5      -   5        $��EnglishVisibleBox��5      $��3q�60   $��English��6;   �� 5�      -   5        $��GermanVisibleBox��5�      $��3q�6/   $��German��6:   �� 5�      -   5        $��SpanishVisibleBox��5�      $��3q�60   $��Spanish��6;   �� 5�      -   5        $��ItalianVisibleBox��5�      $��3q�60   $��Italian��6;   �� 5�      -	   5        $��DutshVisibleBox��5�      $��3q�6.   $��Dutsh��69   �� 5�      -
   5        $��PortugeseVisibleBox��5�      $��3q�62   $��	Portugese��6=   �� 5�      -   5        $��RussishVisibleBox��5�      $��3q�60   $��Russish��6;   �� 5�      -   5        $��JapanishVisibleBox��5�      $��3q�61   $��Japanish��6<   5�      ]� 5�   
   $��5��6   $��3q�6!   .+�5�      �� $��Find ...��6   $��3q�6(   $��Center=�63   �� 5�      , �� SelectedIndex$, 0��6   $��Right=�6)   $��3q�6@   $��80��6K   - �� SelectedValuePath$- Content��6V   . �� SelectedValue. 6r   �� *
SearchType+$ TwoWay=�.@�5�      �� .��6   Key6$   �� 5�      .��6   All6$   �� 5�      .��6   French6'   �� 5�      .��6   	English6(   �� 5�      .��6   German6'   �� 5�      .��6   Dutsh6&   �� 5�      .��6   	Italian6(   �� 5�      .��6   	Spanish6(   �� 5�      .��6   	Portugese6*   �� 5�      .��6   	Russish6(   �� 5�      .��6   
Japanish6)   5�      �� 5�      $
��Next��6   $��80��6$   $��Right=�6/   $��3q�6F   ��6Q   �� *FindNextCommand+�� 5�      $��Find !��6   $��80��6&   $��Right=�61   $��3q�6H   ��6S   �� *FindCommand+�� 5�      -   5        $��3q�5�      ��6!   �� *
SearchText+$ TwoWay=�$ PropertyChanged=�6o   5�      5�      5�      