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
 * Created by Admin on 28.12.2016.
 */

public class ServiceChatAdapter extends RecyclerView.Adapter<ServiceChatAdapter.Holder>{

    private List<ListItem> listData;
    private LayoutInflater inflater;

    private ItemClickCallback itemClickCallback;

    public interface ItemClickCallback{

        void onItemClick(int p);
        void onSecondaryIconClick(int p);
    }

    public void setItemClickCallback(final ItemClickCallback itemClickCallback){

        this.itemClickCallback = itemClickCallback;
    }

    public ServiceChatAdapter(List<ListItem> listData, Context c){

        this.inflater = LayoutInflater.from(c);
        this.listData = listData;
    }

    @Override
    public Holder onCreateViewHolder(ViewGroup parent, int viewType) {
        View view = inflater.inflate(R.layout.list_item, parent, false);
        return new Holder(view);
    }

    @Override
    public void onBindViewHolder(Holder holder, int position) {
        ListItem item = listData.get(position);
        holder.title.setText(item.getTitle());
        holder.name.setText(item.getName());
        if(item.isFavorite()){
            holder.secondaryIcon.setImageResource(R.drawable.ic_star_black_24dp);
        }else{
            holder.secondaryIcon.setImageResource(R.drawable.ic_star_border_black_24dp);
        }
    }

    public void setListData(ArrayList<ListItem> nekiList){
        this.listData.clear();
        this.listData.addAll(nekiList);
    }

    @Override
    public int getItemCount() {
        return listData.size();
    }

    class Holder extends RecyclerView.ViewHolder implements View.OnClickListener{

        private TextView title;
        private TextView name;
        private ImageView thumbnail;
        private ImageView secondaryIcon;
        private View container;

        public Holder(View itemView) {
            super(itemView);

            title = (TextView)itemView.findViewById(R.id.twItemTextMessage);
            name = (TextView)itemView.findViewById(R.id.twItemTextMessageAuthor);
            thumbnail = (ImageView)itemView.findViewById(R.id.iwItemImage);
            secondaryIcon = (ImageView)itemView.findViewById(R.id.iwItemIconSecondary);
            secondaryIcon.setOnClickListener(this);
            container = itemView.findViewById(R.id.contItemRoot);
            container.setOnClickListener(this);
        }

        @Override
        public void onClick(View v) {
            if(v.getId() == R.id.contItemRoot){
                itemClickCallback.onItemClick(getAdapterPosition());
            }else{
                itemClickCallback.onSecondaryIconClick(getAdapterPosition());
            }
        }
    }

}
