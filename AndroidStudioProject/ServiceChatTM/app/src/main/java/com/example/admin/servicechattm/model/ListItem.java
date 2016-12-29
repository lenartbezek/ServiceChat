package com.example.admin.servicechattm.model;

/**
 * Definira en element v recycler view-u
 */

public class ListItem {
    private int imageResId;
    private String messageAuthor;
    private String message;
    private boolean favourite = false;

    public String getSubTitle() {
        return messageAuthor;
    }

    public void setSubTitle(String subTitle) {
        this.messageAuthor = subTitle;
    }

    public boolean isFavourite() {
        return favourite;
    }

    public void setFavourite(boolean favourite) {
        this.favourite = favourite;
    }

    public int getImageResId() {
        return imageResId;
    }

    public void setImageResId(int imageResId) {
        this.imageResId = imageResId;
    }

    public String getTitle() {
        return message;
    }

    public void setTitle(String title) {
        this.message = title;
    }
}