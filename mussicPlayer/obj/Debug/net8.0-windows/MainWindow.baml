   M S B A M L   `   `   `  ����  mussicPlayer    MusicPlayer.MainWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  Music Player��5   	   $	��450��6   $	��800��6+   .��5      �  � ColumnDefinitions 5   
   �� 5      $��* �6   �� 5      $��2* �6   .+�5   
   �� -   5         ��Name$ Playlist��5      $��0��6$   $��10,74,10,10q�6a   �� 5   
   $��1��6   $��10q�6%   .+�5      �� -   5         ��Name$ SongInfo��5      $��16(�6*   $
��Bold"�68   $��5q�6J   �� 5      -   5        $��	Add Songs��5      $��5q�6@   �� 5      -   5        $��Remove Song��5      $��5q�6D   �� 5       �� Orientation$ 
Horizontal=�6   $��5q�62   .+�5      �� -   5        $
��Play��5      $��50��6;   �� 5      -   5        $��Pause��5      $��50��6=   �� 5      -   5        $
��Next��5      $��50��6;   �� 5      -   5        $��Previous��5      $��70��6C   5      �� 5      $��
Play Mode:��6   $��5q�6*   	� 5      -	   5         	�Name$ SequentialMode��5      $��
Sequential��62    d� 	IsChecked$
 TrueY�6G   $��5q�6X   	� 5      -
   5        $ ShuffleMode��5      $��Shuffle��6/   $��5q�6A   �� 5"      -   5         ��Name$ PlaybackSlider��5"       � Minimum$ 0Q�6-   	 � Maximum$		 100Q�69   
 � Value$
 0Q�6G   $��5q�6|   �� 5#      -   5        $ CurrentTimeTextBlock��5#      $��5q�66   �� 5$      -   5        $ TotalTimeTextBlock��5$      $��5q�64   5%      �� 5&   
   $��	List song��6   $
��Left=�6$   $��26��6?   $��	60,19,0,0q�6K   $	��Top=�6^   $	��127��6v   5'      5(      