package com.example.admin.servicechattm.adapter;

import android.content.Context;
import android.support.v7.widget.RecyclerView;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ImageView;
import android.widget.TextView;

import com.example.admin.servicechattm.R;
import com.example.admin.servicechattm.model.ListItem;

import java.util.ArrayList;
import java.util.List;

/**
 * Adapter za recycler view
 */

public class ServiceChatAdapter extends RecyclerView.Adapter<ServiceChatAdapter.Holder>{

    private List<ListItem> listData;
    private LayoutInflater inflater;

    private ItemClickCallback itemClickCallback;

    public interface ItemClickCallback {
        void onItemClick(int p);
        void onSecondaryIconClick(int p);
    }

    public void setItemClickCallback(final ItemClickCallback itemClickCallback) {
        this.itemClickCallback = itemClickCallback;
    }

    public ServiceChatAdapter(List<ListItem> listData, Context c){
        inflater = LayoutInflater.from(c);
        this.listData = listData;
    }

    public void setListData(ArrayList<ListItem> exerciseList) {
        this.listData.clear();
        this.listData.addAll(exerciseList);
    }

    @Override
    public ServiceChatAdapter.Holder onCreateViewHolder(ViewGroup parent, int viewType) {
        View view = inflater.inflate(R.layout.list_item, parent, false);
        return new Holder(view);
    }

    @Override
    public void onBindViewHolder(Holder holder, int position) {
        ListItem item = listData.get(position);
        holder.title.setText(item.getTitle());
        holder.subTitle.setText(item.getSubTitle());
        if (item.isFavourite()){
            holder.secondaryIcon.setImageResource(R.drawable.ic_star_black_24dp);
        } else {
            holder.secondaryIcon.setImageResource(R.drawable.ic_star_border_black_24dp);
        }
    }

    @Override
    public int getItemCount() {
        return listData.size();
    }

    class Holder extends RecyclerView.ViewHolder implements View.OnClickListener{

        ImageView thumbnail;
        ImageView secondaryIcon;
        TextView title;
        TextView subTitle;
        View container;

        public Holder(View itemView) {
            super(itemView);
            thumbnail = (ImageView)itemView.findViewById(R.id.iwAuthorIcon);
            secondaryIcon = (ImageView)itemView.findViewById(R.id.iwSecondaryIcon);
            secondaryIcon.setOnClickListener(this);
            subTitle = (TextView)itemView.findViewById(R.id.twAuthorName);
            title = (TextView)itemView.findViewById(R.id.twMessage);
            container = (View)itemView.findViewById(R.id.cont_item_root);
            container.setOnClickListener(this);
        }

        @Override
        public void onClick(View v) {
            if (v.getId() == R.id.cont_item_root){
                itemClickCallback.onItemClick(getAdapterPosition());
            } else {
                itemClickCallback.onSecondaryIconClick(getAdapterPosition());
            }
        }
    }
}